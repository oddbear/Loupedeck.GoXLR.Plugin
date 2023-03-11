using System.Linq;
using Loupedeck.GoXLR.Plugin.GoXLR.Extensions;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;
using Newtonsoft.Json;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Commands
{
    public class SubscribeToRouting : CommandBase
    {
        public SubscribeToRouting()
        {
            var json = JsonConvert.SerializeObject(new
            {
                @event = "goxlrConnectionEvent",
                payload = Routing.GetRoutingTable()
                    .Select(routing => new { action = "com.tchelicon.goxlr.routingtable", context = $"{routing.Input.GetDescription()}|{routing.Output.GetDescription()}" })
                    .ToArray()
            });

            Json = new [] { json };
        }
    }
}