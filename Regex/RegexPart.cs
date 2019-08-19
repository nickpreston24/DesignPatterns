using System;
using System.Text.RegularExpressions;

namespace RegexBuilder
{
    ///TODO: Make a PatternBuilderVisitor that runs pattern generation when the derived implementation fails
    public abstract class RegexPart : IRegexable
    {
        ///TODO: loop thru all RegexPattern Attributes and validate them
        public bool IsValid() => throw new NotImplementedException();

        protected abstract Regex Generate();

        Regex IRegexable.Generate() => Generate();
    }
}