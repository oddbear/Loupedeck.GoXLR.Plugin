using Newtonsoft.Json;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Commands
{
    public class RespondCanReceiveProfile : CommandBase
    {
        public RespondCanReceiveProfile(string profileName)
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = profileName,
                @event = "didReceiveSettings",
                payload = new
                {
                    settings = new
                    {
                        SelectedProfile = profileName
                    }
                }
            });

            Json = new [] { json };
        }
    }
}