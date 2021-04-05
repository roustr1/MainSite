using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.Services.Birthday
{
    public class BirthdayService : IBirthdayService
    {
        public IEnumerable<UserModel> GetUsers(string urlPath)
        {
#if RELEASE
                var data = DownloadData(new Uri(urlPath)).Result;
            if (data != null)
                return GetUserModels(data);
#endif
            var listTest = new List<UserModel> {
                new UserModel(){ FullFio= "Пирогов А.Р.", PhotoPath = "/images/layout_icons/user.png"},
                new UserModel(){ FullFio="Пирогов А.Р.", PhotoPath ="/images/layout_icons/user.png"},
                new UserModel(){ FullFio="Пирогов А.Р.", PhotoPath ="/images/layout_icons/user.png"},
                new UserModel(){ FullFio="Пирогов А.Р.", PhotoPath ="/images/layout_icons/user.png"},
                new UserModel(){ FullFio="Пирогов А.Р.", PhotoPath ="/images/layout_icons/user.png"}
            };
            return listTest;
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
