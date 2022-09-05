using System.IO;
using System.Text.Json;

namespace Notation.Settings
{
    public class Settings
    {
        private const string SETTINGS_FILENAME = "Notation.config.json";

        public string LastSelectedDirectoryPeriodModels { get; set; }
        public string LastSelectedDirectorySemiTrimesterModels { get; set; }
        public string LastSelectedDirectoryTrimesterModels { get; set; }
        public string LastSelectedDirectoryPeriodReports { get; set; }
        public string LastSelectedDirectorySemiTrimesterReports { get; set; }
        public string LastSelectedDirectoryTrimesterReports { get; set; }
        public string LastSelectedDirectoryYearReports { get; set; }
        public string SQLConnection { get; set; }

        private static Settings _instance;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadSettings();
                }
                return _instance;
            }
        }

        private static Settings LoadSettings()
        {
            return JsonSerializer.Deserialize<Settings>(File.ReadAllText(SETTINGS_FILENAME));
        }

        public static void Save()
        {
            File.WriteAllText(SETTINGS_FILENAME, JsonSerializer.Serialize<Settings>(_instance)
                .Replace("{", "{\r\n\t").Replace("}", "\r\n}").Replace(",", ",\r\n\t"));
        }
    }
}
