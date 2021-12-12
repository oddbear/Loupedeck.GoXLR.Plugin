namespace Loupedeck.GoXLRPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using GoXLR.Server.Enums;
    using GoXLR.Server.Models;

    class RoutingTreeCommand : PluginDynamicCommand
    {
        private GoXLRPlugin _plugin;

        public RoutingTreeCommand() : base()
        {
            this.DisplayName = "Routing Toggle";
            this.GroupName = "";
            this.Description = "Select input and output to toggle.";
            
            this.MakeProfileAction("tree");
        }

        protected override Boolean OnLoad()
        {
            _plugin = (GoXLRPlugin)base.Plugin;
            return true;
        }

        protected override void RunCommand(String actionParameter)
        {
            var split = actionParameter.Split('|');

            if (!Routing.TryParseDescription(split[0], split[1], out var routing))
                return;

            var server = _plugin.Server;
            server.SetRouting(RoutingAction.Toggle, routing, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            this.ActionImageChanged();
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

            // return tree data

            return tree;
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) =>
            $"{actionParameter}";
    }
}
