using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class EmbeddedResources
    {
        public static StreamReader GetEmbeddedResourceStream(this Assembly assembly, string name)
        {
            foreach (string resourceName in assembly.GetManifestResourceNames() ?? Enumerable.Empty<string>())
            {
                if (resourceName.EndsWith(name))
                {
                    return new StreamReader(assembly.GetManifestResourceStream(resourceName));
                }
            }

            return null;
        }

        public static string GetEmbeddedResourceContent(this Assembly assembly, string resourceName)
        {
            var reader = assembly.GetEmbeddedResourceStream(resourceName);

            if (reader == null)
            {
                return string.Empty;
            }

            string data = reader.ReadToEnd();
            reader.Close();

            return data;
        }

        public static string GetAsEmbeddedResource(this string resourceName)
        {
            return GetEmbeddedResourceContent(typeof(EmbeddedResources).Assembly, resourceName);
        }
    }
}
