namespace GoXLR.Server.Models
{
    public class ConnectedClient
    {
        public static ConnectedClient Empty => new ConnectedClient(string.Empty);

        public string Name { get; set; }

        public ConnectedClient(string name)
        {
            Name = name;
        }
    }
}
