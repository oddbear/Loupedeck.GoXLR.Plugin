namespace GoXLR.Server.Handlers.Commands
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fetching profiles from the selected GoXLR App.
    /// </summary>
    internal class RequestProfilesCommand : CommandBase
    {
        public RequestProfilesCommand()
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.profilechange",
                context = "fetchingProfiles",
                @event = "propertyInspectorDidAppear"
            });

            Json = new[] { json };
        }
    }
}
