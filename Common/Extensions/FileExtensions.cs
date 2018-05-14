using IWshRuntimeLibrary;
using Shell32;
using System;
using System.IO;
using System.Reflection;

namespace Common
{
    public static partial class CommonExtensions
    {
        public static string ResolveRelativePath(string referencePath, string relativePath)
        {
            return Path.GetFullPath(Path.Combine(referencePath, relativePath));
        }

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
