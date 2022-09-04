using System.Windows.Forms;

namespace Notation.Utils
{
    public static class FileUtils
    {
        public static string SelectDirectory(string directory, string parameter)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                Description = "Sélectionnez un répertoire",
                SelectedPath = directory,
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                directory = dialog.SelectedPath;

                switch (directory)
                {
                    case "LastSelectedDirectoryPeriodModels":
                        Settings.Settings.Instance.LastSelectedDirectoryPeriodModels = directory;
                        break;
                    case "LastSelectedDirectoryPeriodReports":
                        Settings.Settings.Instance.LastSelectedDirectoryPeriodReports = directory;
                        break;
                    case "LastSelectedDirectorySemiTrimesterModels":
                        Settings.Settings.Instance.LastSelectedDirectorySemiTrimesterModels = directory;
                        break;
                    case "LastSelectedDirectorySemiTrimesterReports":
                        Settings.Settings.Instance.LastSelectedDirectorySemiTrimesterReports = directory;
                        break;
                    case "LastSelectedDirectoryTrimesterModels":
                        Settings.Settings.Instance.LastSelectedDirectoryTrimesterModels = directory;
                        break;
                    case "LastSelectedDirectoryTrimesterReports":
                        Settings.Settings.Instance.LastSelectedDirectoryTrimesterReports = directory;
                        break;
                    case "LastSelectedDirectoryYearReports":
                        Settings.Settings.Instance.LastSelectedDirectoryYearReports = directory;
                        break;
                }
                Settings.Settings.Save();
            }
            else
            {
                directory = "";
            }

            return directory;
        }
    }
}
