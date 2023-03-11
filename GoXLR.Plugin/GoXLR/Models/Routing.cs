using System;
using System.Collections.Generic;
using System.Linq;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.Extensions;

namespace Loupedeck.GoXLR.Plugin.GoXLR.Models
{
    public class Routing
    {
        private const string Seperator = "|"; //The routing seperator
        
        public RoutingInput Input { get; set; }
        public RoutingOutput Output { get; set; }
        public RoutingMode? Mode { get; set; }

        private Routing(RoutingInput input, RoutingOutput output)
        {
            Input = input;
            Output = output;
        }

        private Routing(RoutingInput input, RoutingOutput output, RoutingMode mode)
        {
            Input = input;
            Output = output;
            Mode = mode;
        }

        /// <summary>
        /// Get a combination of every routing input and output
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Routing> GetRoutingTable(bool withMode = false)
        {
            if (withMode)
            {
                return
                    from RoutingOutput output in Enum.GetValues(typeof(RoutingOutput))
                    from RoutingInput input in Enum.GetValues(typeof(RoutingInput))
                    from RoutingMode mode in Enum.GetValues(typeof(RoutingMode))
                    select new Routing(input, output, mode)
                    into routing
                    where routing != new Routing(RoutingInput.Samples, RoutingOutput.Sampler)
                    where routing.Input != RoutingInput.Chat || routing.Output != RoutingOutput.ChatMic
                    select routing;
            }
            
            return
                from RoutingOutput output in Enum.GetValues(typeof(RoutingOutput))
                from RoutingInput input in Enum.GetValues(typeof(RoutingInput))
                select new Routing(input, output)
                into routing
                where routing != new Routing(RoutingInput.Samples, RoutingOutput.Sampler)
                where routing.Input != RoutingInput.Chat || routing.Output != RoutingOutput.ChatMic
                select routing;
        }

        /// <summary>
        /// Get a combination of every routing output with the given input
        /// </summary>
        /// <param name="input">The routing input</param>
        /// <returns></returns>
        public static List<Routing> GetRoutingTable(RoutingInput input)
        {
            return (
                from RoutingOutput output
                in Enum.GetValues(typeof(RoutingOutput))
                select new Routing(input, output)
                into routing
                where routing != new Routing(RoutingInput.Samples, RoutingOutput.Sampler)
                where routing.Input != RoutingInput.Chat || routing.Output != RoutingOutput.ChatMic
                select routing
                ).ToList();
        }
        
        /// <summary>
        /// Get a combination of every routing input with the given output
        /// </summary>
        /// <param name="output">The routing output</param>
        /// <returns></returns>
        public static List<Routing> GetRoutingTable(RoutingOutput output)
        {
            return (
                from RoutingInput input
                in Enum.GetValues(typeof(RoutingInput))
                select new Routing(input, output)
                into routing
                where routing.Input != RoutingInput.Samples || routing.Output != RoutingOutput.Sampler
                where routing.Input != RoutingInput.Chat || routing.Output != RoutingOutput.ChatMic
                select routing
                ).ToList();
        }

        /// <summary>
        /// Get a list of every routing input option
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRoutingInputOptions()
        {
            return (
                from RoutingInput input
                in Enum.GetValues(typeof(RoutingInput))
                select input.GetDescription()
                ).ToList();
        }
        
        /// <summary>
        /// Get a list of every routing output option
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRoutingOutputOptions()
        {
            return (
                from RoutingOutput output
                in Enum.GetValues(typeof(RoutingOutput))
                select output.GetDescription()
                ).ToList();
        }

        /// <summary>
        /// Get a list of every routing action option
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRoutingModeOptions()
        {
            return (
                from RoutingMode mode
                in Enum.GetValues(typeof(RoutingMode))
                select mode.ToString()
                ).ToList();
        }

        /// <summary>
        /// Try parse the context as a routing
        /// </summary>
        /// <param name="context">The context that should be parsed</param>
        /// <param name="routing">The parsed routing</param>
        /// <returns></returns>
        public static bool TryParseContext(string context, out Routing routing)
        {
            var segments = context.Split(Seperator);

            if (Enum.TryParse<RoutingInput>(segments[0].Replace(" ", ""), out var input)
                && Enum.TryParse<RoutingOutput>(segments[1].Replace(" ", ""), out var output))
            {
                if ((input == RoutingInput.Samples && output == RoutingOutput.Sampler)
                    || (input == RoutingInput.Chat && output == RoutingOutput.ChatMic))
                {
                    routing = default;
                    return false;
                }

                if (segments.Length == 3 && Enum.TryParse<RoutingMode>(segments[2].Replace(" ", "").Replace("Turn", ""), out var mode))
                {
                    routing = new Routing(input, output, mode);
                    return true;
                }

                routing = new Routing(input, output);
                return true;
            }

            routing = default;
            return false;
        }

        /// <summary>
        /// Try convert input, output and mode string to a routing
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="output">Output string</param>
        /// <param name="routing">The converted routing</param>
        /// <returns></returns>
        public static bool TryConvert(string input, string output, out Routing routing)
        {
            var context = $"{input}{Seperator}{output}{Seperator}{RoutingMode.Toggle}";

            if (!TryParseContext(context, out var routingTwo))
            {
                routing = default;
                return false;
            }

            routing = routingTwo;
            return true;
        }
        
        /// <summary>
        /// Try convert input, output and mode string to a routing
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="output">Output string</param>
        /// <param name="action">Action string</param>
        /// <param name="routing">The converted routing</param>
        /// <returns></returns>
        public static bool TryConvert(string input, string output, string action, out Routing routing)
        {
            var context = $"{input}{Seperator}{output}{Seperator}{action}";

            if (!TryParseContext(context, out var routingTwo))
            {
                routing = default;
                return false;
            }

            routing = routingTwo;
            return true;
        }

        /// <summary>
        /// Check if the input and output string is a valid option
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="output">Output string</param>
        /// <returns></returns>
        public static bool IsValid(string input, string output)
        {
            return TryParseContext($"{input}{Seperator}{output}", out _);
        }

        /// <summary>
        /// Convert a routing to a string
        /// </summary>
        /// <param name="withAction">If the action should be included in the string</param>
        /// <returns></returns>
        public string ToString(bool withAction = false)
        {
            return withAction ? $"{Input}{Seperator}{Output}{Seperator}{Mode.GetDescription()}" : $"{Input}{Seperator}{Output}";
        }
    }
}