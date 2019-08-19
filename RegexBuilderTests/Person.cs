using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexBuilder
{
    ///Common test class
    public class Person : RegexPart, IEquatable<Person>
    {
        public string FirstName { get; set; }
        public uint Age { get; set; }
        public string Country { get; set; }
        public double GPA { get; set; }

        public override bool Equals(object obj) => Equals(obj as Person);

        public bool Equals(Person other)
            => other != null
            && FirstName == other.FirstName
            && Age == other.Age
            && Country == other.Country
            && GPA == other.GPA;

        public override int GetHashCode() => HashCode.Combine(FirstName, Age, Country, GPA);

        public override string ToString()
            => new StringBuilder()
            .AppendLine($"{nameof(FirstName)}: {FirstName}")
            .AppendLine($"{nameof(Age)}: {Age}")
            .AppendLine($"{nameof(GPA)}: {GPA}")
            .AppendLine($"{nameof(Country)}: {Country}")
            .ToString();

        protected override Regex Generate() => throw new NotImplementedException();

        public static bool operator ==(Person left, Person right) => EqualityComparer<Person>.Default.Equals(left, right);

        public static bool operator !=(Person left, Person right) => !(left == right);
    }
}