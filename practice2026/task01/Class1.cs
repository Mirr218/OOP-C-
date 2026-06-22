using System.Text;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string str)
        {
            if (str == null) return false;

            string input = str.ToLower();
            StringBuilder sb = new StringBuilder();
            foreach(char c in input)
            {
                if (char.IsWhiteSpace(c) || char.IsPunctuation(c)) { continue; }
                else { sb.Append(c); }
            }

            string cleaned = sb.ToString();
            return cleaned == String.Concat(cleaned.Reverse());
        }
    }
}
