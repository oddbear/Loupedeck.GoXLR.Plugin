namespace Loupedeck.GoXLRPlugin
{
    using System;
    using System.Linq;
    using System.Net.NetworkInformation;

    using Enums;

    using GoXLR.Server;
    using GoXLR.Server.Configuration;
    using GoXLR.Server.Models;

    using StructureMap;
    
    public class GoXLRPlugin : Plugin
    {
        public override Boolean UsesApplicationApiOnly => true;
        public override Boolean HasNoApplication => true;

        public IContainer Container { get; private set; }
        public GoXLRServer Server { get; private set; }
        public PluginState PluginState { get; private set; }

        public override void Load()
        {
            this.LoadPluginIcons();
            this.SetDefaultPluginState();

            this.Container = new Container(cfg =>
            {
                cfg.For<GoXLRPlugin>().Use(this);
                cfg.For<IGoXLREventHandler>().Use<GoXLREventHandler>().Singleton();
                cfg.IncludeRegistry<GoXLRServerRegistry>();
            });
            
            this.Server = this.Container.GetInstance<GoXLRServer>();
            this.Server.Start();
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

        private void SetDefaultPluginState()
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var activeTcpConnections = ipGlobalProperties.GetActiveTcpListeners();
            var isPortInUse = activeTcpConnections.Any(tcpConnectionInformation => tcpConnectionInformation.Port == 6805);

            //TODO: If the Port is in use the notification will not magically disappear,
            // when this port is freed (everything else works ok though).
            var pluginState = isPortInUse
                ? PluginState.PortInUse
                : PluginState.AppNotConnected;
            
            this.SetPluginState(pluginState);
        }

        public void SetPluginState(PluginState state)
        {
            //Set current state:
            this.PluginState = state;

            //Notify state changed:
            switch (state)
            {
                case PluginState.AppConnected:
                    base.OnPluginStatusChanged(Loupedeck.PluginStatus.Normal, "GoXLR App is connected.", "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/");
                    break;
                case PluginState.AppNotConnected:
                    base.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "GoXLR App is not connected.", "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/");
                    break;
                case PluginState.PortInUse:
                    base.OnPluginStatusChanged(Loupedeck.PluginStatus.Error, "Port 6805 is already in use.", "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/");
                    break;
            }
        }
    }
}
