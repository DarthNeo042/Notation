using System;

namespace Notation.Utils
{
    public static class NameUtils
    {
        public static string FormatPascal(string name)
        {
            string[] names = name.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string nameSplit in names)
            {
                string formattedNameSplit = nameSplit.Substring(0, 1).ToUpper();
                if (name.Length > 1)
                {
                    formattedNameSplit += nameSplit.Substring(1).ToLower();
                }
                name = name.Replace(nameSplit, formattedNameSplit);
            }

            return name;
        }
    }
}
