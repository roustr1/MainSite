using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Application.Dal.Infrastructure;
using Microsoft.EntityFrameworkCore;

// https://github.com/Razario/EntityFrameworkIndex

namespace Application.Dal
{
    public class ConfigDb 
    {
        private readonly ApplicationContext _context;

        public ConfigDb(ApplicationContext context)
        {
            _context = context;
        }

        private int? _language;

        /// <summary>
        /// Код языка для полнотекстового поиска
        /// По умолчанию используется "Русский"
        /// </summary>
        public int Language
        {
            get
            {
                return _language.HasValue ? _language.Value : 1049; //1049 - русский язык
            }
            set { _language = value; }
        }

        /// <summary>
        /// Пересчитать индексы
        /// </summary>
        public void CalculateIndexes()
        {
            if (GetCompleteFlag()) return;

            //Получаем все сущности текущего DbContext
            var t = _context.GetType().GetProperties();
            foreach (var property in _context.GetType().GetProperties().Where(f => f.PropertyType.BaseType != null && f.PropertyType.Name == "DbSet`1"))
            {
                var currentEntityType = property.PropertyType.GetGenericArguments().FirstOrDefault();
                if (currentEntityType == null)
                    continue;

                //Получаем название таблицы в БД
                var tableAttribute = currentEntityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
                var tableName = tableAttribute != null ? tableAttribute.Name : property.Name;

                //Получаем у сущности свойства помеченые аттрибутом FullTextIndex, создаем по ним индекс
                BuildingFullTextIndexes(tableName, currentEntityType.GetProperties().Where(f => f.GetCustomAttributes(typeof(FullTextIndexAttribute), false).Any()));
            }

            CreateCompleteFlag();
        }
        
        /// <summary>
        /// Создание индексов
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="propertyes">Коллекция свойств сущности</param>
        private void BuildingIndexes(string tableName, IEnumerable<PropertyInfo> propertyes)
        {
            foreach (var property in propertyes)
            {
                _context.Database.ExecuteSqlRaw(String.Format("CREATE INDEX IX_{0} ON {1} ({0})", property.Name, tableName)); //Создаем индекс
            }
        }

        /// <summary>
        /// Создание полнотекстовых индексов
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="propertyes">Коллекция свойств сущности</param>
        private void BuildingFullTextIndexes(string tableName, IEnumerable<PropertyInfo> propertyes)
        {
            if (propertyes == null) return;
            if (!propertyes.Any()) return;

            var fullTextColumns = string.Empty;
            foreach (var property in propertyes)
            {
                fullTextColumns += String.Format("{0}{1} language {2}", (string.IsNullOrWhiteSpace(fullTextColumns) ? null : ","), property.Name, Language);
            }

            //Создаем полнотекстовый индекс
            _context.Database.ExecuteSqlRaw(string.Format("IF NOT EXISTS (SELECT * FROM sysindexes WHERE id=object_id('{1}') and name='IX_{2}') CREATE UNIQUE INDEX IX_{2} ON {1} ({2});CREATE FULLTEXT CATALOG FTXC_{1} AS DEFAULT;CREATE FULLTEXT INDEX ON {1}({0}) KEY INDEX [IX_{2}] ON ([FTXC_{1}]) WITH STOPLIST = SYSTEM;", fullTextColumns, tableName, "Id"));
        }

        private void CreateCompleteFlag()
        {
            //EF6.0 

            //_context.Database.ExecuteSqlRaw("CREATE TABLE [dbo].[__IndexBuildingHistory]([DataContext] [nvarchar](255) NOT NULL, [Complete] [bit] NOT NULL, CONSTRAINT [PK___IndexBuildingHistory] PRIMARY KEY CLUSTERED ([DataContext] ASC))");
        }

        private bool GetCompleteFlag()
        {
            #warning найти способ узнать существует ли индекс
            
            //EF6.0 

            //var queryResult = Database.SqlQuery(typeof(string), "IF OBJECT_ID('__IndexBuildingHistory', 'U') IS NOT NULL SELECT 'True' AS 'Result' ELSE SELECT 'False' AS 'Result'").GetEnumerator();
            //queryResult.MoveNext();
            //Database.Connection.Close();

            return false;
        }
    }
}