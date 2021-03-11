using System.Collections.Generic;
using Application.Dal.Domain.Settings;

namespace Application.Services.Settings
{
    public interface ISettingsService
    {
        Setting GetParameterValue(string name);

        /// <summary>
        /// устанавливает новое значение для параметра. если параметра нет, создает его
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        Setting SetParameter(Setting setting);

        /// <summary>
        /// устанавливает новое значение для параметра. если параметра нет, создает его
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Setting SetParameter(string name, string value);

        /// <summary>
        /// удаление параметра
        /// </summary>
        /// <param name="id"></param>
        void DeleteSetting(string id);

        /// <summary>
        ///  удаление параметра
        /// </summary>
        /// <param name="setting"></param>
        void DeleteSetting(Setting setting);

        Dictionary<string, string> GetAllSettingsDictionary { get; }
        ICollection<Setting> GetAllSettings();

        Setting GetSettingById(string id);
    }
}