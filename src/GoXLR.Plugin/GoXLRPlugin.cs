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
        public override bool UsesApplicationApiOnly => true;
        public override bool HasNoApplication => true;

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

        public override void Unload() =>
            this.Server?.Dispose();

        private void OnApplicationStarted(object sender, EventArgs e)
        {
        }

        private void OnApplicationStopped(object sender, EventArgs e)
        {
        }

        public override void RunCommand(string commandName, string parameter)
        {
        }

        public override void ApplyAdjustment(string adjustmentName, string parameter, int diff)
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
                    base.OnPluginStatusChanged(
                        Loupedeck.PluginStatus.Normal,
                        "GoXLR App is connected.",
                        "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/",
                        "GoXLR Plugin Support");
                    break;
                case PluginState.AppNotConnected:
                    base.OnPluginStatusChanged(
                        Loupedeck.PluginStatus.Warning,
                        "GoXLR App is not connected.",
                        "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/",
                        "GoXLR Plugin Support");
                    break;
                case PluginState.PortInUse:
                    base.OnPluginStatusChanged(
                        Loupedeck.PluginStatus.Error,
                        "Port 6805 is already in use.",
                        "https://github.com/oddbear/Loupedeck.GoXLR.Plugin/",
                        "GoXLR Plugin Support");
                    break;
            }
        }
    }
}
