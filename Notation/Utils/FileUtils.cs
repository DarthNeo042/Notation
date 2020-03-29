using System;
using System.Windows.Forms;
using Notation.ViewModels;

namespace Notation.Utils
{
    public static class FileUtils
    {
        public static string SelectDirectory()
        {
            string directory = "";

            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                Description = "Sélectionnez un répertoire",
                // TODO
                //SelectedPath = MainViewModel.Singleton.Cookie.LastDirectory,
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                directory = dialog.SelectedPath;
                // TODO
                //MainViewModel.Singleton.Cookie.LastDirectory = directory;
            }

            return directory;
        }
    }
}
