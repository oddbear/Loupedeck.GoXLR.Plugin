namespace GoXLR.Server.Handlers.Commands
{
    using Enums;
    using Extensions;
    using GoXLR.Server.Models;

    using Newtonsoft.Json;

    internal class SetRoutingCommand : CommandBase
    {
        public SetRoutingCommand(RoutingAction action, Routing routing)
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.routingtable",
                @event = "keyUp",
                payload = new
                {
                    settings = new
                    {
                        RoutingAction = action.ToString(),
                        RoutingInput = routing.Input.GetEnumDescription(),
                        RoutingOutput = routing.Output.GetEnumDescription()
                    }
                }
            });

            Json = new[] { json };
        }
    }
}
