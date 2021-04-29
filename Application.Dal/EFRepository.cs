using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Dal.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly ApplicationContext _context;
        private readonly string _emptyGuid = Guid.Empty.ToString();

        public EfRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(CheckAndCreateGuid(entity));
            _context.SaveChanges();
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<TEntity>().Add(CheckAndCreateGuid(entity));
            }
            _context.SaveChanges();
        }
        
        public void Update(TEntity entity)
        {
            if (entity == null) return;

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void Delete(string id)
        {
            if (id == null) 
                throw new ArgumentNullException("id is null");
            var entity = Get(id);
            if (entity == null)
                throw new ArgumentNullException("entity is null");
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null) return;
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public TEntity Get(string id)
        {
            return _context.Set<TEntity>().FirstOrDefault(element=>element.Id==id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _context.Set<TEntity>().Find(where);
        }

        public IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> @where)
        {
            return _context.Set<TEntity>().Where(@where);
        }

        public IEnumerable<TEntity> GetAll
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }
      
         
        
        
        private TEntity CheckAndCreateGuid(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id) || entity.Id == _emptyGuid)
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            return entity;
        }
    }
}