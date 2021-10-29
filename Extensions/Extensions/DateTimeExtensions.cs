namespace Common.Extensions
{
    public static partial class DateTimeExtensions
    {
        public static DateTime? ParseOrEmpty(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            DateTime.TryParse(text, out var result);
            return result;
        }
    }
}