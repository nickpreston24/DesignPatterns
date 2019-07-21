using IWshRuntimeLibrary;
using Shell32;
using System.Reflection;
using System.Security.Cryptography;

namespace System.IO
{
    public static partial class Extensions
    {
        public static bool Equals(this FileInfo first, FileInfo second, bool useHash = false) => useHash
                ? FilesAreEqual_Hash(first, second)
                : FilesAreEqual_OneByte(first, second);

        private static bool FilesAreEqual_OneByte(this FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
                return true;

            using (var fs1 = first.OpenRead())
            using (var fs2 = second.OpenRead())
            {
                for (int i = 0; i < first.Length; i++)
                {
                    if (fs1.ReadByte() != fs2.ReadByte())
                        return false;
                }
            }

            return true;
        }

        private static bool FilesAreEqual_Hash(this FileInfo first, FileInfo second)
        {
            byte[] firstHash = MD5.Create().ComputeHash(first.OpenRead());
            byte[] secondHash = MD5.Create().ComputeHash(second.OpenRead());

            for (int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static string ResolveRelativePath(string referencePath, string relativePath) => Path.GetFullPath(Path.Combine(referencePath, relativePath));

        public static void CreateStartupFolderShortcut()
        {
            var wshShell = new WshShellClass();
            IWshShortcut shortcut;
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Create the shortcut
            shortcut =
              (IWshShortcut)wshShell.CreateShortcut(
                startUpFolderPath + "\\" +
                Assembly.GetExecutingAssembly().FullName
                //Application.ProductName + ".lnk"
                );

            //shortcut.TargetPath = Application.ExecutablePath;
            //shortcut.WorkingDirectory = Application.StartupPath;

            string exePath = Assembly.GetExecutingAssembly().GetName().CodeBase;
            shortcut.TargetPath = exePath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(exePath);
            shortcut.Description = "Launch My Application";

            // shortcut.IconLocation = Application.StartupPath + @"\App.ico";
            shortcut.Save();
        }

        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = Path.GetFileName(shortcutFilename);

            Shell32.Shell shell = new ShellClass();
            var folder = shell.NameSpace(pathOnly);
            var folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                var link =
                  (ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        public static void DeleteStartupFolderShortcuts(string targetExeName)
        {
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            var di = new DirectoryInfo(startUpFolderPath);
            var files = di.GetFiles("*.lnk");

            foreach (var fi in files)
            {
                string shortcutTargetFile = GetShortcutTargetFile(fi.FullName);

                if (shortcutTargetFile.EndsWith(targetExeName,
                      StringComparison.InvariantCultureIgnoreCase))
                {
                    System.IO.File.Delete(fi.FullName);
                }
            }
        }
    }
}
