using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Application.Dal;
using Application.Dal.Domain.Birthday;
using Newtonsoft.Json;

namespace Application.Services.Birthday
{
    public class BirthdayService : IBirthdayService
    {
        private IRepository<Birtday> _birtdayRepository;

        public BirthdayService(IRepository<Birtday> birtdayRepository)
        {
            _birtdayRepository = birtdayRepository;
        }
        public IEnumerable<Birtday> GetTodayBirth()
        {
            var today = DateTime.Today.Day;
            var currentMonth = DateTime.Today.Month;
            var collection = _birtdayRepository.GetAll.Where(c => c.Birth.Day == today && c.Birth.Month == currentMonth);

            foreach (var birtday in collection)
            {
                birtday.FIO = ShortName(birtday.FIO);
            }
            return collection;
        }

        private string ShortName(string fio)
        {
            string[] str = fio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (str.Length != 3) throw new ArgumentException("ФИО задано в неверно формате");
            return $"{str[0]} {str[1][0]}. {str[2][0]}.";
        }



        /// <summary>
        /// Получение данных с API
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<byte[]> DownloadData(Uri path)
        {
            using var client = new WebClient();
            try
            {
                var data = await client.DownloadDataTaskAsync(path);
                return data;
            }
            catch (WebException)
            {
                return null;
            }
        }

        /// <summary>
        /// Конвертирование данных полученных с API в отображаемый формат
        /// </summary>
        /// <param name="retrievedData"></param>
        /// <returns></returns>
        private IEnumerable<UserModel> GetUserModels(byte[] retrievedData)
        {
            var result = new List<UserModel>();
            var data = JsonConvert.DeserializeObject<UserModel[]>(Encoding.GetEncoding("UTF-8")
                .GetString(retrievedData, 0, retrievedData.Length));
            return data.ToList();
        }
    }
}
