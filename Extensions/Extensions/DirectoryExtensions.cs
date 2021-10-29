namespace Common.Extensions
{
    public static partial class DirectoryExtensions
    {
        public static string GoUp(this string folderPath, int numDirs = 1) =>
            Directory.Exists(folderPath)
                ? Path.GetFullPath(Path.Combine(folderPath, string.Join(string.Empty, Enumerable.Repeat(@"..\", numDirs))))
                : folderPath;
    }
}