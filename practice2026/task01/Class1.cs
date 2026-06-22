using System.Text;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            string str = input.ToLower();
            StringBuilder sb = new StringBuilder();
            foreach(char c in str)
            {
                if (char.IsWhiteSpace(c) || char.IsPunctuation(c)) { continue; }
                else { sb.Append(c); }
            }

            string cleaned = sb.ToString();
            if (String.IsNullOrEmpty(cleaned)) return false;
            return cleaned == String.Concat(cleaned.Reverse());
        }
    }
}
