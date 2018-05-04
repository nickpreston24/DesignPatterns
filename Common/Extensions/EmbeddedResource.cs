using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class EmbeddedResources
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static StreamReader GetEmbeddedResourceStream(this Assembly assembly, string name)
        {
            try
            {
                foreach (string resourceName in assembly.GetManifestResourceNames() ?? Enumerable.Empty<string>())
                {
                    if (resourceName.EndsWith(name))
                    {
                        return new StreamReader(assembly.GetManifestResourceStream(resourceName));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                _logger.Error(ex);
            }

            return null;
        }

        public static string GetEmbeddedResourceContent(this Assembly assembly, string resourceName)
        {
            try
            {
                var reader = assembly.GetEmbeddedResourceStream(resourceName);

                if (reader == null)
                {
                    return String.Empty;
                }

                string data = reader.ReadToEnd();
                reader.Close();

                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                _logger.Error(ex);
            }

            return String.Empty;
        }

        public static string GetAsEmbeddedResource(this string resourceName)
        {
            return GetEmbeddedResourceContent(typeof(EmbeddedResources).Assembly, resourceName);
        }
    }
}
