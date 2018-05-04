namespace Common.Extensions
{
    public static partial class Extensions
    {
        public static string ToLetter(this bool value, bool isUpper) => value ? isUpper ? "T" : "t" : isUpper ? "F" : "f";
        public static string ToYesOrNo(this bool value, bool isLetter, bool isUpper)
        {
            if (isLetter)
            {
                return value ? isUpper ? "Y" : "y" : isUpper ? "N" : "n";
            }
            else
            {
                return value ? isUpper ? "Yes" : "yes" : isUpper ? "No" : "no";
            }
        }
    }
}
