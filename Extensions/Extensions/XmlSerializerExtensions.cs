namespace Common.Extensions
{
    public class XmlSerializerExtensions
    {
        private static readonly Dictionary<RuntimeTypeHandle, XmlSerializer> serializers = new Dictionary<RuntimeTypeHandle, XmlSerializer>();

        /// <summary>
        ///   Serialize object to xml string by <see cref = "XmlSerializer" />
        /// </summary>
        public static string ToXml<T>(this T value)
        //where T : class//new()
        {
            var serializer = GetSerializer(typeof(T));
            using (var stream = new MemoryStream())
            using (var writer = new XmlTextWriter(stream, new UTF8Encoding()))
            {
                serializer.Serialize(writer, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static string ToPrettyXml(this string xml)
        {
            if (!IsXml(xml))
                throw new ArgumentException("Invalid xml string", nameof(xml));

            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///   Serialize object to stream by <see cref = "XmlSerializer" />
        /// </summary>
        public static void ToXml<T>(this T value, Stream stream)
            where T : new()
        {
            var serializer = GetSerializer(typeof(T));
            serializer.Serialize(stream, value);
        }

        /// <summary>
        ///   Deserialize object from string
        /// </summary>    
        public static T FromXml<T>(this string xml)
            where T : new()
        {
            var serializer = GetSerializer(typeof(T));
            using (var stringReader = new StringReader(xml))
            using (XmlReader reader = new XmlTextReader(stringReader))
                return (T)serializer.Deserialize(reader);
        }

        /// <summary>
        ///   Deserialize object from stream
        /// </summary>    
        public static T FromXml<T>(this Stream source)
            where T : new()
        {
            var serializer = GetSerializer(typeof(T));
            return (T)serializer.Deserialize(source);
        }

        private static XmlSerializer GetSerializer(Type type)
        {
            if (serializers.TryGetValue(type.TypeHandle, out XmlSerializer serializer))
                return serializer;

            lock (serializers)
            {
                if (!serializers.TryGetValue(type.TypeHandle, out serializer))
                {
                    serializer = new XmlSerializer(type);
                    serializers.Add(type.TypeHandle, serializer);
                }
            }

            return serializer;
        }

        public static void WriteXML(this string xml, string directoryPath, string title, string extension = "xml")
        {
            if (!IsXml(xml))
                throw new ArgumentException("Invalid xml string", nameof(xml));

            if (string.IsNullOrEmpty(directoryPath))
                throw new ArgumentException(nameof(directoryPath));

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string path = Path.Combine(directoryPath, $"{title}_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}") + $".{extension}";
            string specialCharacters = "[\\|<>?*:/\"]";
            Regex.Replace(path, specialCharacters, string.Empty);

            xml.ReplaceAll(Constants.RemovableSpecialXMLCharacters);

            File.WriteAllText(path, xml);
        }

        public static bool IsXml(this string xml) => !string.IsNullOrEmpty(xml) && xml.StartsWith("<");
    }
}