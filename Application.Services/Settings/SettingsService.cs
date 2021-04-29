using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Application.Dal;
using Application.Dal.Domain.Settings;

namespace Application.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        
        private readonly IRepository<Setting> _settingsRepository;

        public SettingsService(IRepository<Setting> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public Setting GetParameterValue(string name) => _settingsRepository.GetAll.FirstOrDefault(c => c.Name == name);

        public Setting SetParameter(Setting setting)
        {
            if (setting.Id == null) _settingsRepository.Add(setting);
            else
            {
                var entity = _settingsRepository.Get(setting.Id);
                if (entity == null)
                    _settingsRepository.Add(setting);
                else _settingsRepository.Update(setting);
            }
            return setting;

        }

        public Setting SetParameter(string name, string value)
        {
            var setting = GetParameterValue(name);
            if (setting != null)//если  нашли параметр в бд
            {
                setting.Value = value;
                _settingsRepository.Update(setting);
            }
            else
            {
                setting = new Setting { Name = name, Value = value };
                _settingsRepository.Add(setting);
            }

            return setting;

        }

        public void DeleteSetting(string id)
        {
            _settingsRepository.Delete(_settingsRepository.Get(id));

        }

        public void DeleteSetting(Setting setting) => _settingsRepository.Delete(setting);

        public Dictionary<string, string> SettingsDictionary
        {
            get
            {
                var collection = new Dictionary<string, string>();
                foreach (var kvp in _settingsRepository.GetAll)
                {
                    collection[kvp.Name] = kvp.Value;
                }
                return collection;
            }
        }

        public string SetDictionaryParameter(string keyName, string value = null)
        {
            if (!SettingsDictionary.TryGetValue(keyName, out var val))
            {
                SettingsDictionary.Add(keyName, value);
            }
            return value;
        }

        public Setting GetSettingById(string id) => _settingsRepository.Get(id);

        public ICollection<Setting> GetAllSettings() => _settingsRepository.GetAll.ToList();
    }
}