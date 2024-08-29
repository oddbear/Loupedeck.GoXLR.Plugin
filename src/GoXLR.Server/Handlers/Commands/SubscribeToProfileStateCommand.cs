namespace GoXLR.Server.Handlers.Commands
{
    using GoXLR.Server.Models;

    using Newtonsoft.Json;

    internal class SubscribeToProfileStateCommand : CommandBase
    {
        public SubscribeToProfileStateCommand(Profile profile)
        {
            //It's required to send a propertyInspectorDidAppear at least once, not sure why.
            var propertyInspectorDidAppear = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = profile.Name,
                @event = "propertyInspectorDidAppear"
            });
            var willAppear = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = profile.Name,
                @event = "willAppear"
            });

            Json = new [] { propertyInspectorDidAppear, willAppear };
        }
    }
}
