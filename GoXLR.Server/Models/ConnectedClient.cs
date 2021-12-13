namespace GoXLR.Server.Models
{
    public readonly struct ConnectedClient
    {
        public static ConnectedClient Empty => new ConnectedClient(string.Empty);

        public string Name { get; }

        public ConnectedClient(string name)
        {
            Name = name;
        }

        public static bool operator == (ConnectedClient left, ConnectedClient right)
            => left.Equals(right);

        public static bool operator != (ConnectedClient left, ConnectedClient right)
            => !left.Equals(right);
    }
}
