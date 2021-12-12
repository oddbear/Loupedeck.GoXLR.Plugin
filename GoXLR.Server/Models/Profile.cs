namespace GoXLR.Server.Models
{
    public struct Profile
    {
        public static Profile Empty => new Profile(string.Empty);

        public string Name { get; set; }

        public Profile(string name)
        {
            Name = name;
        }

        public static bool operator ==(Profile left, Profile right)
            => left.Equals(right);

        public static bool operator !=(Profile left, Profile right)
            => !left.Equals(right);
    }
}
