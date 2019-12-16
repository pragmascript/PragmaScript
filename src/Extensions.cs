namespace PragmaScript
{
    public static class Extensions
    {
        public static char at(this string s, int idx)
        {
            if (idx >= 0 && idx < s.Length)
            {
                return s[idx];
            }
            else
            {
                return '\0';
            }
        }
    }
}