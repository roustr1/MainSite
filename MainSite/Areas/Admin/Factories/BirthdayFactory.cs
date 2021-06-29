using Application.Dal.Domain.Birthday;
using Application.Dal.Infrastructure;
using Application.Services.Files;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MainSite.Areas.Admin.Factories
{
    public class BirthdayFactory : IBirthdayFactory
    {
        private readonly IAppFileProvider _fileProvider;
        private readonly IFileDownloadService _downloadService;

        public BirthdayFactory(IFileDownloadService downloadService, IAppFileProvider appFileProvider)
        {
            _downloadService = downloadService;
            _fileProvider = appFileProvider;
        }

        public List<Birtday> ParseFile(IFormFile file)
        {
           return ParsingXmlFile(GetPathLoadFile(file));
        }

        private string GetPathLoadFile(IFormFile load)
        {
            if (load != null)
            {
                _fileProvider.WriteAllBytes(_fileProvider.GetAbsolutePath("files", load.FileName), _downloadService.GetDownloadBits(load));
                //сохраняем файл в папку на сервере
                string xsltPath = Path.Combine(_fileProvider.GetAbsolutePath("files", load.FileName));
                return xsltPath;
            }

            return null;
        }

        private List<Birtday> ParsingXmlFile(string path)
        {
            if (path == null) return null;

            // начало использования библиотеке ClosedXML
            var workbook = new XLWorkbook(path);
            List<Birtday> result = new List<Birtday>();

            foreach (var worksheet in workbook.Worksheets)
            {
                result = GenerationLisBirthday(worksheet);
            }

            return result;
           
        }

        private List<Birtday> GenerationLisBirthday(IXLWorksheet worksheet)
        {
            List<Birtday> list = new List<Birtday>();
            var rows = worksheet.RangeUsed().RowsUsed();
            var columnIndexUser = -1;
            var columnIndexBirthdayUser = -1;
            var columnIndexSubDivisionUser = -1;
            var columnIndexLast = -1;
            bool isStart = false;

            for (int i = 0; i < rows.Count(); i++)
            {
                if (!isStart && CheckStartingFiled(rows.ElementAt(i).Cell(1).Value))
                {
                    var currentRow = rows.ElementAt(i);
                    int j = 2;
                    columnIndexLast = getColumnIndexLast(rows.ElementAt(i));

                    isStart = true;

                    while (true)
                    {
                        if (j > 20) break;

                        if (currentRow.Cell(j).Value.ToString().ToUpper().Trim() == "СОТРУДНИК")
                        {
                            columnIndexUser = j;
                            if (columnIndexBirthdayUser != -1 && columnIndexSubDivisionUser != -1) break;
                        }

                        if (currentRow.Cell(j).Value.ToString().ToUpper().Trim() == "ДАТА РОЖДЕНИЯ")
                        {
                            columnIndexBirthdayUser = j;
                            if (columnIndexSubDivisionUser != -1 && columnIndexUser != -1) break;
                        }

                        if(currentRow.Cell(j).Value.ToString().ToUpper().Trim() == "ПОДРАЗДЕЛЕНИЕ")
                        {
                            columnIndexSubDivisionUser = j;
                            if (columnIndexBirthdayUser != -1 && columnIndexUser != -1) break;
                        }

                        j++;
                    }

                    i++;
                }
                else
                {
                    if (isStart)
                    {
                        var valueUser = rows.ElementAt(i).Cell(columnIndexUser).Value.ToString().Trim();
                        var valueBirthdayUser = rows.ElementAt(i).Cell(columnIndexBirthdayUser).Value.ToString().Trim();
                        var valueSubdivisionUser = rows.ElementAt(i).Cell(columnIndexSubDivisionUser).Value.ToString().Trim();
                        if ((!String.IsNullOrWhiteSpace(valueUser) && !String.IsNullOrWhiteSpace(valueBirthdayUser))) { 
                            var prevIndexRow = i;
                            while(String.IsNullOrWhiteSpace(rows.ElementAt(prevIndexRow).Cell(columnIndexLast).Value.ToString().Trim()))
                            {
                                prevIndexRow--;
                            }

                            var departmentShortName = rows.ElementAt(prevIndexRow).Cell(columnIndexLast).Value.ToString().Trim();

                            list.Add(new Birtday { 
                                Birth = DateTime.Parse(valueBirthdayUser),
                                FIO = valueUser,
                                DepartmentShortName = departmentShortName,
                                DepartmentFullName = valueSubdivisionUser,
                                Id = Guid.NewGuid().ToString()
                            });
                        }
                    }
                } 
            }

            return list;
        }

        private int getColumnIndexLast(IXLRangeRow row)
        {
            var indexLastColumn = 1;
            while (true)
            {
                if (row.Cell(indexLastColumn).MergedRange() != null)
                {
                    var elementsCellsRange = row.Cell(indexLastColumn).MergedRange().Cells().ToList().Count(a => !String.IsNullOrWhiteSpace(a.GetString()));

                    if (elementsCellsRange > 0)
                    {
                        var countMergeColumn = row.Cell(indexLastColumn).MergedRange() == null ? 1 : row.Cell(indexLastColumn).MergedRange().ColumnCount();
                        indexLastColumn += countMergeColumn;
                    }
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(row.Cell(indexLastColumn).GetString()))
                    {
                        indexLastColumn++;
                    }
                    else
                    {
                        break;
                    }
                }
               
            }

            return indexLastColumn;
        }

        private bool CheckStartingFiled(object value)
        {
            var result = value.ToString();
            Regex regex = new Regex(@"\s");
            var check = regex.Replace(result, "");
            if (!String.IsNullOrWhiteSpace(result) && check.Equals("№п/п")) return true;

            return false;
        }
    }
}
