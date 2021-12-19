namespace GoXLR.Server.Handlers.Commands
{
    using Newtonsoft.Json;

    internal class RespondCanReceiveProfileStateCommand : CommandBase
    {
        public RespondCanReceiveProfileStateCommand(string profileName)
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
