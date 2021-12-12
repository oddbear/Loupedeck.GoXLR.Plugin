namespace GoXLR.Server.Models
{
    public class Profile
    {
        public static Profile Empty => new Profile(string.Empty);

        public string Name { get; set; }

        public Profile(string name)
        {
            Name = name;
        }
    }
}
