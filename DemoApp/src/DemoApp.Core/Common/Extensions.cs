namespace DemoApp.Core.Common
{
    public static class Extensions
    {
        public static string TrimStart(this string target, string trimChars)
        {
            return target.TrimStart(trimChars.ToCharArray());
        }

        public static string TrimEnd(this string target, string trimChars)
        {
            return target.TrimEnd(trimChars.ToCharArray());
        }
    }
}
