namespace Loupedeck.GoXLRPlugin
{
    using System;

    using GoXLR.Server;
    using GoXLR.Server.Configuration;
    using GoXLR.Server.Models;

    using StructureMap;

    using Plugin = Loupedeck.Plugin;

    public class GoXLRPlugin : Plugin
    {
        public override Boolean UsesApplicationApiOnly => true;
        public override Boolean HasNoApplication => true;

        public IContainer Container { get; private set; }
        public GoXLRServer Server { get; private set; }

        public override void Load()
        {
            LoadPluginIcons();

            Container = new Container(cfg =>
            {
                cfg.For<GoXLRPlugin>().Use(this);
                cfg.For<IGoXLREventHandler>().Use<GoXLREventHandler>().Singleton();
                cfg.IncludeRegistry<GoXLRServerRegistry>();
            });

            Container.GetInstance<IGoXLREventHandler>().ConnectedClientChangedEvent(ConnectedClient.Empty);

            Server = Container.GetInstance<GoXLRServer>();
            Server.Start();
        }

        public override void Unload()
        {
        }

        private void OnApplicationStarted(Object sender, EventArgs e)
        {
        }

        private void OnApplicationStopped(Object sender, EventArgs e)
        {
        }

        public override void RunCommand(String commandName, String parameter)
        {
        }

        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff)
        {
        }

        private void LoadPluginIcons()
        {
            //var resources = this.Assembly.GetManifestResourceNames();
            this.Info.Icon16x16 = EmbeddedResources.ReadImage("Loupedeck.GoXLRPlugin.Resources.Icons.Icon-16.png");
            this.Info.Icon32x32 = EmbeddedResources.ReadImage("Loupedeck.GoXLRPlugin.Resources.Icons.Icon-32.png");
            this.Info.Icon48x48 = EmbeddedResources.ReadImage("Loupedeck.GoXLRPlugin.Resources.Icons.Icon-48.png");
            this.Info.Icon256x256 = EmbeddedResources.ReadImage("Loupedeck.GoXLRPlugin.Resources.Icons.Icon-256.png");
        }
    }
}
