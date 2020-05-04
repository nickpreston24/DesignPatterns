namespace .
{
    public static partial class RegexExtensions
    {
        /// <summary>
        /// Replaces all entries within the map for a given line.
        /// </summary>        
        public static string ReplaceAll(this string line, Dictionary<string, string> replacementMap)
        {
            // Non-throwing
            if (string.IsNullOrEmpty(line))
                return string.Empty;

            return replacementMap is null || replacementMap.Count == 0
                ? line
                : Regex.Replace(line, string.Join("|", replacementMap.Keys
                    .Select(keyName => keyName.ToString())
                    .ToArray()),
            match =>
            {
                replacementMap.TryGetValue(match.Value, out string val);
                return val ?? string.Empty;
            });
        }
    }
}