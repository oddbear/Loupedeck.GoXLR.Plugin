using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.Extensions;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;
using Newtonsoft.Json;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Commands
{
    public class RespondCanReceiveRouting : CommandBase
    {
        public RespondCanReceiveRouting(string context, Routing routing)
        {
            var json = JsonConvert.SerializeObject(new
            {
                action = "com.tchelicon.goxlr.routingtable",
                context,
                @event = "didReceiveSettings",
                payload = new
                {
                    settings = new
                    {
                        RoutingAction = (routing.Mode ?? RoutingMode.Toggle).ToString(),
                        RoutingInput = routing.Input.GetDescription(),
                        RoutingOutput = routing.Output.GetDescription()
                    }
                }
            });

            Json = new [] { json };
        }
    }
}