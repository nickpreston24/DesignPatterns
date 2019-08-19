using System.Text.RegularExpressions;

namespace RegexBuilder
{
    public interface IRegexable
    {
        bool IsValid();

        Regex Generate();
    }
}