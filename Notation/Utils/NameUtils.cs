namespace Notation.Utils
{
    public static class NameUtils
    {
        public static string FormatPascalFirstName(string name)
        {
            string[] names;

            if (name.Contains("-"))
            {
                names = name.Split(new char[] { '-' });
            }
            else if (name.Contains(" "))
            {
                names = name.Split(new char[] { ' ' });
            }
            else
            {
                names = new string[] { name };
            }

            string formattedName = "";

            foreach (string _name in names)
            {
                if (!string.IsNullOrEmpty(formattedName))
                {
                    formattedName += "-";
                }
                if (name.Length > 0)
                {
                    formattedName += _name.Substring(0, 1).ToUpper();
                }
                if (name.Length > 1)
                {
                    formattedName += _name.Substring(1, _name.Length - 1).ToLower();
                }
            }

            return formattedName;
        }

        public static string FormatPascal(string name)
        {
            string formattedName = "";
            if (!string.IsNullOrEmpty(name))
            {
                formattedName += name.Substring(0, 1).ToUpper();
            }
            if (name.Length > 1)
            {
                formattedName += name.Substring(1).ToLower();
            }
            return formattedName;
        }
    }
}
