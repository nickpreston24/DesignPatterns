using System;

namespace RegexBuilder
{
    public class RegexPatternAttribute : Attribute
    {
        private string patternText;

        public RegexPatternAttribute(string patternText)
        {
            this.patternText = patternText;
            if (!IsValid())
                throw new Exception(nameof(patternText));
        }

        private bool IsValid() => true;
    }
}