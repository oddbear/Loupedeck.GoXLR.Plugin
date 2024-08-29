namespace Loupedeck.GoXLRPlugin
{
    using System.Linq;

    public class DynamicActionProvider
    {
        private readonly GoXlrPlugin _plugin;

        public DynamicActionProvider(GoXlrPlugin plugin)
        {
            _plugin = plugin;
        }

        public TCommand Provide<TCommand>()
            where TCommand : PluginDynamicAction
        {
            return _plugin.DynamicCommands
                .OfType<TCommand>()
                .Single();
        }
    }
}
