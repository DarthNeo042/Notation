using Notation.Properties;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

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
                string configFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Notation.exe.config");

                XmlDocument document = new XmlDocument();
                document.Load(configFilename);
                foreach (XmlNode node in document.GetElementsByTagName("setting"))
                {
                    if (node.Attributes["name"].Value == parameter)
                    {
                        node.FirstChild.FirstChild.Value = directory;
                    }
                }
                document.Save(configFilename);

                Settings.Default.Reload();
            }
            else
            {
                directory = "";
            }

            return directory;
        }
    }
}
