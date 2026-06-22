using System.Text;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (input == null) return false;

            string str = input.ToLower();
            StringBuilder sb = new StringBuilder();
            foreach(char c in str)
            {
                if (char.IsWhiteSpace(c) || char.IsPunctuation(c)) { continue; }
                else { sb.Append(c); }
            }

            string cleaned = sb.ToString();
            return cleaned == String.Concat(cleaned.Reverse());
        }
    }
}
