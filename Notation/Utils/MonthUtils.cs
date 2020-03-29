namespace Notation.Utils
{
    public static class MonthUtils
    {
        private static readonly string[] _months = { "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" };

        public static string Name(int month)
        {
            return _months[month - 1];
        }
    }
}
