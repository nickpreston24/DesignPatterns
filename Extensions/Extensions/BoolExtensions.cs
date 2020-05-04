namespace Common.Extensions
{
    public static partial class Extensions
    {
        public static string ToLetter(this bool value, bool capitalize = true) => value ? capitalize ? "T" : "t" : capitalize ? "F" : "f";
        public static string ToYesOrNo(this bool value, bool isLetter = true, bool capitalize = true)
        {
            if (isLetter)
            {
                return value ? capitalize ? "Y" : "y" : capitalize ? "N" : "n";
            }
            else
            {
                return value ? capitalize ? "Yes" : "yes" : capitalize ? "No" : "no";
            }
        }

        public static bool ToBoolean(this string text) =>
            (text.ToLower().Equals("yes") || text.ToLower().Equals("y"))
            ? true
            : (text.ToLower().Equals("no") || text.ToLower().Equals("n")) ? false : true;
    }
}
