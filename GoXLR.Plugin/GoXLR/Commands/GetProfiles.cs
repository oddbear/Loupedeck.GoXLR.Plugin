using Newtonsoft.Json;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Commands
{
    public class GetProfiles : CommandBase
    {
        public GetProfiles()
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = "fetchingProfiles",
                @event = "propertyInspectorDidAppear"
            });

            Json = new [] { json };
        }
    }
}