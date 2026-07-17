using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace RdpOctopus.Services
{
    public class SettingsService<TSetting>
    {
        private readonly string _settingsPath;

        public SettingsService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "RdpOctopus");

            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            _settingsPath = Path.Combine(appDataPath, typeof(TSetting).Name);
        }

        public TSetting Load()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    return JsonConvert.DeserializeObject<TSetting>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки настроек: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return default(TSetting);
        }

        public void Save(TSetting settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings);
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения настроек: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
