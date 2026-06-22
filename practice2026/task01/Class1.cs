using System.Text;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string str)
        {
            string input = str.ToLower();
            StringBuilder sb = new StringBuilder();
            foreach(char c in input)
            {
                if (char.IsWhiteSpace(c) || char.IsPunctuation(c)) { continue; }
                else { sb.Append(c); }
            }
            return sb.ToString() == input;
        }
    }
}
