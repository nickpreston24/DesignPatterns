namespace RegexBuilder
{
    public class FullName
    {
        [RegexPattern(@"\d+^[,!?]")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public override string ToString()
            => $"{FirstName} {MiddleName} {LastName} {Suffix}";

        public static implicit operator string(FullName fullName)
            => fullName.ToString();

        public static implicit operator FullName(string fullName)
        {
            string[] parts = fullName.Split(' ', ',', '\t', '\n', '\r');
            return new FullName
            {
                FirstName = parts.Get(0),
                MiddleName = parts.Get(1),
                LastName = parts.Get(2),
                Suffix = parts.Get(3),
            };
        }
    }
}