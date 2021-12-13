namespace Loupedeck.GoXLRPlugin.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;

    using GoXLR.Server.Enums;
    using GoXLR.Server.Models;

    using Helpers;

    class SetRoutingCommand : PluginDynamicCommand
    {
        private readonly Dictionary<String, State> _states = new Dictionary<String, State>();

        private GoXLRPlugin _plugin;

        public SetRoutingCommand() : base()
        {
            this.DisplayName = "Routing Toggle";
            this.GroupName = "";
            this.Description = "Select input and output to toggle.";
            
            this.MakeProfileAction("tree");
        }

        protected override Boolean OnLoad()
        {
            this._plugin = (GoXLRPlugin)base.Plugin;
            return true;
        }

        public void UpdateState(Routing routing, State state)
        {
            var actionParameter = $"{routing.Input}|{routing.Output}";
            if (this._states.TryGetValue(actionParameter, out var oldState) && oldState == state)
                return;

            this._states[actionParameter] = state;

            this.ActionImageChanged(actionParameter);
        }
        
        protected override void RunCommand(String actionParameter)
        {
            if (!Routing.TryParseContext(actionParameter, out var routing))
                return;

            var server = this._plugin.Server;
            server.SetRouting(RoutingAction.Toggle, routing, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Routing Tree");

            tree.AddLevel("Inputs");
            tree.AddLevel("Outputs");

            var routingTable = Routing.GetRoutingTable();
            var inputs = routingTable
                .Select(routing => routing.Input)
                .Distinct();
            
            foreach (var input in inputs)
            {
                var node = tree.Root.AddNode(input.ToString());

                var outputs = routingTable
                    .Where(routing => routing.Input == input)
                    .Select(routing => routing.Output);

                foreach (var output in outputs)
                {
                    var description = GoXLR.Server.Extensions.EnumExtensions.GetEnumDescription(output);

                    node.AddItem($"{input}|{output}", output.ToString(), description);
                }
            }
            
            return tree;
        }
        
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                var background = ImageHelpers.GetImageBackground(imageSize, Color.Red);
                var bitmapImage = new BitmapImage(background);
                bitmapBuilder.SetBackgroundImage(bitmapImage);

                //base: Routing Toggle
                //this: Input|Output\r\n- -> Input|Output\r\nOn
                var text = this.GetCommandDisplayName(actionParameter, imageSize);
                bitmapBuilder.DrawText(text);

                return bitmapBuilder.ToImage();
            }
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            //Does not work...
            if (actionParameter is null)
                return string.Empty;

            var text = actionParameter;
            if (actionParameter.IndexOf('|') != -1)
            {
                var split = actionParameter.Split('|');
                text = $"{split[0]}\r\n{split[1]}";
            }

            return this._states.TryGetValue(actionParameter, out var state)
                ? $"{text} \r\n {state}"
                : $"{text} \r\n -";
        }
    }
}
