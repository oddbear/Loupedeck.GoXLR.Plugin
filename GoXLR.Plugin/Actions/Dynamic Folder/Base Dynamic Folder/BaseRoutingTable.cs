using System;
using System.Collections.Generic;
using Loupedeck.GoXLR.Plugin.Config;
using Loupedeck.GoXLR.Plugin.GoXLR;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.EventArgs;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;
using Loupedeck.GoXLR.Plugin.Helpers;
using EventHandler = Loupedeck.GoXLR.Plugin.GoXLR.EventHandler;

namespace Loupedeck.GoXLR.Plugin.Actions.Dynamic_Folder.Base_Dynamic_Folder
{
    public abstract class BaseRoutingTable : PluginDynamicFolder
    {
        protected RoutingOutput Output;

        /// <summary>
        /// Once loaded subscribe to RoutingChanged events
        /// </summary>
        /// <returns></returns>
        public override bool Load()
        {
            EventHandler.OnRoutingChanged += OnRoutingChanged;
            EventHandler.RefreshIcons += RefreshIcons;
            return base.Load();
        }

        /// <summary>
        /// Once unloaded unsubscribe from RoutingChanged events
        /// </summary>
        /// <returns></returns>
        public override bool Unload()
        {
            EventHandler.OnRoutingChanged -= OnRoutingChanged;
            EventHandler.RefreshIcons -= RefreshIcons;
            return base.Unload();
        }

        /// <summary>
        /// Only open the Folder if the GoXLR Software is connected
        /// </summary>
        /// <returns></returns>
        public override bool Activate()
        {
            if (Plugin.PluginStatus.Status == PluginStatus.Normal)
                return base.Activate();
            
            Close();
            return false;

        }
        
        /// <summary>
        /// Set the Type of the Dynamic Folder Navigation
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public override PluginDynamicFolderNavigation GetNavigationArea(DeviceType _)
        {
            return PluginDynamicFolderNavigation.ButtonArea;
        }
        
        /// <summary>
        /// Dynamically add every routing option to the folder for the given output
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
        {
            var routing = new List<string>();
            foreach (var cmdRouting in Routing.GetRoutingTable(Output))
            {
                routing.Add(CreateCommandName(cmdRouting.ToString()));
            }
            return routing;
        }
        
        /// <summary>
        /// Generate an Image for the routing option if the actionParameter can be parsed
        /// </summary>
        /// <param name="actionParameter"></param>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        public override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
        {
            if (!Routing.TryParseContext(actionParameter, out var routing))
                return base.GetCommandImage(actionParameter, imageSize);

            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                GoXlrPlugin.RoutingStates.TryGetValue(routing.ToString(), out var state);

                var background = ImageHelper.GetRoutingImageForFolder(
                    imageSize,
                    routing,
                    state,
                    GoXlrConfig.Routing.Color.Font,
                    GoXlrConfig.Routing.Color.Active,
                    GoXlrConfig.Routing.Color.InActive,
                    GoXlrConfig.Routing.Color.Background,
                    GoXlrConfig.Routing.Color.NotAvailable,
                    GoXlrConfig.Routing.ActiveIconPath,
                    GoXlrConfig.Routing.InActiveIconPath
                    );
                
                bitmapBuilder.SetBackgroundImage(BitmapImage.FromArray(background));
                
                return bitmapBuilder.ToImage();
            }
        }
        
        /// <summary>
        /// Only Run when the GoXLR Software is connected and the routing can be parsed
        /// </summary>
        /// <param name="actionParameter"></param>
        public override async void RunCommand(string actionParameter)
        {
            if (!Routing.TryParseContext(actionParameter, out var routing))
                return;

            await CommandHandler.Send(
                new GoXLR.Commands.SetRouting(routing),
                GoXlrPlugin.WsConnection
                );
        }
        
        /// <summary>
        /// Once the routing changed, invoke a refresh of the Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRoutingChanged(object sender, OnRoutingChangedEventArgs e)
        {
            CommandImageChanged(e.Routing.ToString());
        }
        
        /// <summary>
        /// Invoke that every image has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void RefreshIcons(object sender, EventArgs e)
        {
            CommandImageChanged(null);
        }
    }
}