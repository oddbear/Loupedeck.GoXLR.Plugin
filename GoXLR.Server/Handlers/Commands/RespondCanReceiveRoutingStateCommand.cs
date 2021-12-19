namespace GoXLR.Server.Handlers.Commands
{
    using Enums;
    using Extensions;
    using GoXLR.Server.Models;

    using Newtonsoft.Json;

    internal class RespondCanReceiveRoutingStateCommand : CommandBase
    {
        public RespondCanReceiveRoutingStateCommand(string context, Routing routing)
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.routingtable",
                context = context,
                @event = "didReceiveSettings",
                payload = new
                {
                    settings = new
                    {
                        RoutingAction = RoutingAction.Toggle.ToString(),
                        RoutingInput = routing.Input.GetEnumDescription(),
                        RoutingOutput = routing.Output.GetEnumDescription()
                    }
                }
            });

            Json = new [] { json };
        }
    }
}
