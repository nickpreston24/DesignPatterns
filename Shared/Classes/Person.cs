using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Classes
{
    ///Common test class
    public class Person : IEquatable<Person>
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
            && GPA == other.GPA
            && Country == other.Country;

        public override int GetHashCode()
        {
            var hashCode = 75998067;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + Age.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Country);
            return hashCode;
        }

        public override string ToString()
            => new StringBuilder()
            .AppendLine($"{nameof(FirstName)}: {FirstName}")
            .AppendLine($"{nameof(Age)}: {Age}")
            .AppendLine($"{nameof(GPA)}: {GPA}")
            .AppendLine($"{nameof(Country)}: {Country}")
            .ToString();

        public static bool operator ==(Person left, Person right)
            => EqualityComparer<Person>.Default.Equals(left, right);

        public static bool operator !=(Person left, Person right)
            => !(left == right);
    }
}