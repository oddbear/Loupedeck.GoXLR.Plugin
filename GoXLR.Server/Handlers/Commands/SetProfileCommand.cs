using GoXLR.Server.Models;

namespace GoXLR.Server.Handlers.Commands
{
    using Newtonsoft.Json;

    internal class SetProfileCommand : CommandBase
    {
        public SetProfileCommand(Profile profile)
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                @event = "keyUp",
                payload = new
                {
                    settings = new
                    {
                        SelectedProfile = profile.Name
                    }
                }
            });

            Json = new[] { json };
        }
    }
}
