namespace GoXLR.Server.Handlers.Commands
{
    using GoXLR.Server.Models;

    using Newtonsoft.Json;

    internal class UnSubscribeToProfileStateCommand : CommandBase
    {
        public UnSubscribeToProfileStateCommand(Profile profile)
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = profile.Name,
                @event = "willDisappear"
            });

            Json = new [] { json };
        }
    }
}
