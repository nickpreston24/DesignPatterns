namespace System
{
    public class PatternAttribute : Attribute
    {
        private string pattern;
        public string Value => pattern;
        public PatternAttribute(string propertyRegex)
        {
            pattern = propertyRegex;
        }
    }
}
