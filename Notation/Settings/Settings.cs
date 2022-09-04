using System.IO;
using System.Xml.Serialization;

namespace Notation.Settings
{
    public class Settings
    {
        private const string SETTINGS_FILENAME = "Notation.config";

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
            using (FileStream fileStream = new FileStream(SETTINGS_FILENAME, FileMode.Open))
            {
                return (Settings)new XmlSerializer(typeof(Settings)).Deserialize(fileStream);
            }
        }

        public static void Save()
        {
            using (FileStream fileStream = new FileStream(SETTINGS_FILENAME, FileMode.Create))
            {
                new XmlSerializer(typeof(Settings)).Serialize(fileStream, _instance);
            }
        }
    }
}
