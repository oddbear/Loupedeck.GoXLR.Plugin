using Newtonsoft.Json;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Commands
{
    public class SubscribeToProfiles : CommandBase
    {
        public SubscribeToProfiles(string profile)
        {
            var propertyInspectorDidAppear = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = profile,
                @event = "propertyInspectorDidAppear"
            });
            
            var willAppear = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = profile,
                @event = "willAppear"
            });

            Json = new [] { propertyInspectorDidAppear, willAppear };
        }
    }
}