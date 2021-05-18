using Application.Dal.Infrastructure;
using Application.Services.Files;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Application.Services.PlanCalendar
{
    public class PlanCalendarSevice: IPlanCalendarSevice
    {
        private readonly IFileDownloadService _downloadService;
        private readonly IAppFileProvider _fileProvider;

        public PlanCalendarSevice(IFileDownloadService downloadService, IAppFileProvider appFileProvider)
        {
            _downloadService = downloadService;
            _fileProvider = appFileProvider;
        }

        public List<DataBaseParserModel> Start(IFormFile file)
        {
           return ParsingXmlFile(GetPathLoadFile(file));
        }

        public string GetPathLoadFile(IFormFile load)
        {
            if (load != null)
            {
                _fileProvider.WriteAllBytes(_fileProvider.GetAbsolutePath("files", load.FileName), _downloadService.GetDownloadBits(load));
                //_fileProvider.CreateFile(_fileProvider.GetAbsolutePath("files", load.FileName));
                //сохраняем файл в папку на сервере
                string xsltPath = Path.Combine(_fileProvider.GetAbsolutePath("files", load.FileName));
                return xsltPath;
            }

            return null;
        }

        public bool CheckStartingFiled(object value)
        {
            var result = value.ToString();
            Regex regex = new Regex(@"\s");
            var check = regex.Replace(result, "");
            if (!String.IsNullOrWhiteSpace(result) && check.Equals("№п/п")) return true;

            return false;
        }

        public List<DataBaseParserModel> ParsingXmlFile(string path)
        {
            if (path == null) return null;

            // начало использования библиотеке ClosedXML
            var workbook = new XLWorkbook(path);
            List<DataBaseParserModel> dbParser = new List<DataBaseParserModel>();


            foreach (var worksheet in workbook.Worksheets)
            {
                var curList = GenerationListActivitysInCurrentMoth(worksheet);
                if (curList != null) dbParser.Add(GenerationListActivitysInCurrentMoth(worksheet));
            }
            return dbParser;
        }

        private DataBaseParserModel GenerationListActivitysInCurrentMoth(IXLWorksheet worksheet) {
            if (new Regex(@"\d").Matches(worksheet.Name).Count != 4) return null;

            var rows = worksheet.RangeUsed().RowsUsed(); // Skip header row

            var dataBaseParserModel = new DataBaseParserModel();
            dataBaseParserModel.Year = GetYear(worksheet.Name);
            dataBaseParserModel.Month = GetMoth(worksheet.Name);

            for (int i = 0; i < rows.Count(); i++)
            {
                if (CheckStartingFiled(rows.ElementAt(i).Cell(1).Value))
                {
                    var countMergeColumn = rows.ElementAt(i).Cell(1).MergedRange().RowCount();
                    var date = GenerationDayDictionary(rows, i);

                    for (int currentIndexActivity = i + countMergeColumn + 1, k = 2, programIndex = -1; currentIndexActivity < rows.Count(); currentIndexActivity++)
                    {
                        var currentActivity = rows.ElementAt(currentIndexActivity).Cell(k);

                        if (!String.IsNullOrWhiteSpace(currentActivity.Value.ToString()))
                        {
                            foreach (var element in GenerationMarksInCurrentActivity(currentIndexActivity, date, rows))
                            {
                                ParserActivityModel parserActivityModel = new ParserActivityModel();
                                parserActivityModel.DayOfEvent = element.IsExist ? DateTime.Parse(element.day + dataBaseParserModel.Month + dataBaseParserModel.Year).ToString("dd.MM.yyyy") : element.Text;
                                parserActivityModel.Name = currentActivity.Value.ToString();
                                parserActivityModel.NameAllStav = GenerationData(currentActivity.Value.ToString(), currentIndexActivity, date.Last().Key + 1, rows);
                                parserActivityModel.Leader = GenerationData(parserActivityModel.NameAllStav, currentIndexActivity, date.Last().Key + 2, rows);
                                parserActivityModel.LocationOfEvent = GenerationData(parserActivityModel.Leader, currentIndexActivity, date.Last().Key + 3, rows);
                                parserActivityModel.TimeOfEvent = GenerationData(parserActivityModel.LocationOfEvent, currentIndexActivity, date.Last().Key + 4, rows);
                                parserActivityModel.ResultOfEvent = GenerationData(parserActivityModel.TimeOfEvent, currentIndexActivity, date.Last().Key + 5, rows);
                                parserActivityModel.NameProgram = rows.ElementAt(programIndex).Cell(1).Value.ToString();

                                dataBaseParserModel.ParserActivityModelCollection.Add(parserActivityModel);
                            }
                        }
                        else
                        {
                            if (String.IsNullOrWhiteSpace(rows.ElementAt(currentIndexActivity).Cell(1).Value.ToString())) break;
                            programIndex = currentIndexActivity;
                        }
                    }
                    break;
                }
                //break;
            }

            return dataBaseParserModel;
        }

        private string GetMoth(string name)
        {
            Regex regex = new Regex(@"\d");

            return regex.Replace(name.Trim(), "");
        }

        private int GetYear(string name)
        {
            var result = "";
            Regex regex = new Regex(@"\d");
            foreach (var el in regex.Matches(name.Trim())) {
                result += el.ToString();
            }
            return Int32.Parse(result);
        }

        private string GenerationData(string v, int indexRow, int indexColumn, IXLRangeRows rows)
        {
            var result = rows.ElementAt(indexRow).Cell(indexColumn).Value.ToString();
            if (String.IsNullOrWhiteSpace(result) && rows.ElementAt(indexRow).Cell(indexColumn).IsMerged())
            {
                return v;
            }

            return rows.ElementAt(indexRow).Cell(indexColumn).Value.ToString();
        }

        private void GenerationData(ParserActivityModel parserActivityModel, int indexRow, int indexColumn, IXLRangeColumns columns)
        {
            for(int i = indexRow, j = indexColumn; j < columns.Count(); j++)
            {
                parserActivityModel.NameAllStav = columns.ElementAt(j).Cell(i).Value.ToString();
            }
        }

        private List<ParserDateModel> GenerationMarksInCurrentActivity(int currentIndexActivity, Dictionary<int, int> dayDictionary, IXLRangeRows rows)
        {
            List<ParserDateModel> result = new List<ParserDateModel>(); 
            for(var i = 0; i < dayDictionary.Count();)
            {
                var objectDay = rows.ElementAt(currentIndexActivity).Cell(dayDictionary.ElementAt(i).Key);
                if(objectDay.MergedRange() != null)
                {
                    result.Add(
                        new ParserDateModel() {
                            Text = objectDay.Value.ToString(),
                            day = dayDictionary.ElementAt(i).Value,
                            lastDay = dayDictionary.ElementAt(i).Value + objectDay.MergedRange().ColumnCount() - 1
                        });
                    i = i + objectDay.MergedRange().ColumnCount();
                    continue;
                }

                if (!String.IsNullOrWhiteSpace(objectDay.Value.ToString()))
                    result.Add(new ParserDateModel() {
                        IsExist = true,
                        day = dayDictionary.ElementAt(i).Value
                    });
                i++;
            }

            return result;
        }

        private Dictionary<int, int> GenerationDayDictionary(IXLRangeRows rows, int index)
        {
            var j = 2;
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            while (true)
            {
                if (rows.ElementAt(index).Cell(j).MergedRange().ColumnCount() > 1)
                {
                    var newElement = rows.ElementAt(index + 1);
                    while (!String.IsNullOrWhiteSpace(newElement.Cell(j).Value.ToString()))
                    {
                        var tempSymbol = newElement.Cell(j).Address.ColumnNumber;
                        var tempValue = Int32.Parse(newElement.Cell(j).Value.ToString());

                        dictionary[tempSymbol] = tempValue;
                        j++;
                    }
                    break;
                }
                j++;
            }
            return dictionary;
        }
    }
}
