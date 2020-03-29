namespace Notation.Utils
{
    public static class NumberUtils
    {
        private static string[] _rankStrings = { "1er", "2ème", "3ème", "4ème" };

        public static string GetRankString(int number)
        {
            string rankString = "";

            if (number > 0 && number <= _rankStrings.Length)
            {
                rankString = _rankStrings[number - 1];
            }

            return rankString;
        }
    }
}
