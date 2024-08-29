namespace GoXLR.Server.Handlers.Models
{
    using System.Linq;

    using Enums;
    using GoXLR.Server.Models;

    using Newtonsoft.Json.Linq;

    public class MessageNotification
    {
        public string Action { get; set; }
        public string Context { get; set; }
        public string Event { get; set; }
        public JObject Payload { get; set; }

        public State GetStateFromPayload()
        {
            return (State)this.Payload
                .GetValue("state")
                .Value<int>();
        }

        public Profile[] GetProfilesFromPayload()
        {
            return this.Payload
                .GetValue("Profiles")
                .ToObject<string[]>()
                .Select(profileName => new Profile(profileName))
                .ToArray();
        }
    }
}
