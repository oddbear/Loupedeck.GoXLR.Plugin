// ReSharper disable CollectionNeverUpdated.Global

using System.Collections;
using System.Linq;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Models
{
    public class NotificationMessage
    {
        [JsonProperty("action")]
        public string Action { get; set; }
        
        [JsonProperty("context")]
        public string Context { get; set; }
        
        [JsonProperty("event")]
        public string Event { get; set; }
        
        [JsonProperty("payload")]
        public JObject Payload { get; set; } 

        /// <summary>
        /// Get the state out of the payload
        /// </summary>
        /// <returns></returns>
        public State GetState()
        {
            var payload = Payload["state"]?.Value<int>();
            return payload == 0 ? State.On : State.Off;
        }

        /// <summary>
        /// Get the profiles out of the payload
        /// </summary>
        /// <returns></returns>
        public string[] GetProfiles()
        {
            return (from object profile in (IEnumerable)Payload["Profiles"] select profile.ToString()).ToArray();
        }
    }
}