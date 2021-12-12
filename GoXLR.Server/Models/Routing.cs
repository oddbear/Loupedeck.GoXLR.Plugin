using System;
using System.Collections.Generic;
using System.Linq;
using GoXLR.Server.Enums;
using GoXLR.Server.Extensions;

namespace GoXLR.Server.Models
{
    public struct Routing
    {
        public RoutingInput Input { get; set; }
        public RoutingOutput Output { get; set; }

        public Routing(RoutingInput input, RoutingOutput output)
        {
            Input = input;
            Output = output;
        }

        public static bool TryParseDescription(string inputDescription, string outputDescription, out Routing routing)
        {
            try
            {
                //Ex. Line In, Line Out
                if (!EnumExtensions.TryParseEnumFromDescription(inputDescription, out RoutingInput input))
                {
                    routing = default;
                    return false;
                }

                if (!EnumExtensions.TryParseEnumFromDescription(outputDescription, out RoutingOutput output))
                {
                    routing = default;
                    return false;
                }

                routing = new Routing(input, output);
                return true;
            }
            catch
            {
                routing = default;
                return false;
            }

        }
        
        public static bool TryParseContext(string context, out Routing routing)
        {
            try
            {
                //Ex. LineIn|LineOut
                var segments = context.Split(GoXLRServer.RoutingSeparator);
                var input = EnumExtensions.EnumParse<RoutingInput>(segments[0]);
                var output = EnumExtensions.EnumParse<RoutingOutput>(segments[1]);
                routing = new Routing(input, output);
                return true;
            }
            catch
            {
                routing = default;
                return false;
            }
        }

        public static IEnumerable<Routing> GetRoutingTable()
        {
            return
                from input in EnumExtensions.EnumGetValues<RoutingInput>()
                from output in EnumExtensions.EnumGetValues<RoutingOutput>()
                select new Routing(input, output) into routing

                where routing != new Routing(RoutingInput.Chat, RoutingOutput.ChatMic)
                where routing != new Routing(RoutingInput.Samples, RoutingOutput.Sampler)
                select routing;
        }

        public static bool operator ==(Routing left, Routing right)
            => left.Equals(right);

        public static bool operator !=(Routing left, Routing right)
            => !left.Equals(right);
    }
}
