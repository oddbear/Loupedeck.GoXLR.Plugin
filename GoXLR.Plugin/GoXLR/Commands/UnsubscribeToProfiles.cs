using Newtonsoft.Json;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Commands
{
    public class UnsubscribeToProfiles : CommandBase
    {
        public UnsubscribeToProfiles(string profile)
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = profile,
                @event = "willDisappear"
            });
            
            Json = new [] { json };
        }
    }
}