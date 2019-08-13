using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace System
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> GetValues<TEnum>()
            where TEnum : struct, IConvertible, IFormattable, IComparable => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

        public static Dictionary<int, string> ToDictionary(this Enum @enum)
        {
            var type = @enum.GetType();
            return Enum.GetValues(type).Cast<int>().ToDictionary(value => value, value => Enum.GetName(type, value));
        }

        //public static string GetDescription<TEnum>(this TEnum value)
        //    where TEnum : struct, IConvertible, IFormattable, IComparable
        //{
        //    var fieldInfo = typeof(TEnum).GetField(value.ToString());

        //    var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //    return Convert.ToString(((attributes.Length > 0) ? attributes[0].Description : value.ToString()));
        //}

        public static string GetDescription(this Enum value)
        {
            try
            {
                var fieldInfo = value.GetType().GetField(value.ToString());

                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return Convert.ToString(((attributes.Length > 0) ? attributes[0].Description : value.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static TEnum Next<TEnum>(this TEnum source)
            where TEnum : struct, IConvertible, IFormattable, IComparable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(TEnum).FullName));
            }

            var values = (TEnum[])Enum.GetValues(source.GetType());

            int j = Array.IndexOf<TEnum>(values, source) + 1;

            return (values.Length == j) ? values[0] : values[j];
        }

        public static TEnum Previous<TEnum>(this TEnum source)
            where TEnum : struct, IConvertible, IFormattable, IComparable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(TEnum).FullName));
            }

            var values = (TEnum[])Enum.GetValues(source.GetType());

            int j = Array.IndexOf<TEnum>(values, source) - 1;

            return (values.Length == j) ? values[0] : values[j];
        }

        // Intended for enums to create conditions that are comparable to instances with given enum as a property
        public static string ToExpressionEnumCondition<TEnum>(this TEnum enumType)
            where TEnum : struct, IConvertible, IFormattable, IComparable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new NotSupportedException($"Type {typeof(TEnum).Name} is not a valid Enum.");
            }

            var comparableEnumCondition = new StringBuilder();

            comparableEnumCondition.Append("\"");
            comparableEnumCondition.Append(enumType.ToString());
            comparableEnumCondition.Append("\"");

            return comparableEnumCondition.ToString();
        }

        public static TEnum GetRandom<TEnum>()
            where TEnum : struct, IConvertible, IFormattable, IComparable => GetValues<TEnum>().OrderBy(e => Guid.NewGuid()).FirstOrDefault();

        public static TEnum GetRandom<TEnum>(this TEnum @enum)
            where TEnum : struct, IConvertible, IFormattable, IComparable => GetRandom<TEnum>();

    }

    /// <summary>
    /// Enum Binding Source
    /// 
    /// Allows the Binding of any enum you specify via namespace
    /// Example XAML: ItemsSource="{Binding Source={local:EnumBindingSource EnumType=regexns:RegexOptions}}"
    /// Paste This XAML to your WPF: 	
    ///		xmlns:local="clr-namespace:$YourLocalNamespaceHere$"
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type enumType;
        public Type EnumType
        {
            get { return enumType; }
            set
            {
                if (value != enumType)
                {
                    if (null != value)
                    {
                        var enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                        {
                            throw new ArgumentException("Type must be for an Enum.");
                        }
                    }
                    enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType) => EnumType = enumType;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == enumType)
            {
                throw new InvalidOperationException("The EnumType must be specified.");
            }

            var actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            //Get all the enum values to an array:
            var enumValues = Enum.GetValues(actualEnumType);
            //If enum type matches, return:
            if (actualEnumType == enumType)
            {
                return enumValues;
            }

            var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }

    }
}
