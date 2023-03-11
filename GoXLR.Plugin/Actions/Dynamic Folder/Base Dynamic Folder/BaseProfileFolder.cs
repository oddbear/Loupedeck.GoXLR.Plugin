using System;
using System.Collections.Generic;
using Loupedeck.GoXLR.Plugin.Config;
using Loupedeck.GoXLR.Plugin.GoXLR;
using Loupedeck.GoXLR.Plugin.GoXLR.EventArgs;
using Loupedeck.GoXLR.Plugin.Helpers;
using EventHandler = Loupedeck.GoXLR.Plugin.GoXLR.EventHandler;

namespace Loupedeck.GoXLR.Plugin.Actions.Dynamic_Folder.Base_Dynamic_Folder
{
    public abstract class BaseProfileFolder : PluginDynamicFolder
    {
        /// <summary>
        /// Once loaded subscribe to ProfileSelected events
        /// </summary>
        /// <returns></returns>
        public override bool Load()
        {
            EventHandler.OnProfileSelected += OnProfileSelected;
            EventHandler.RefreshIcons += RefreshIcons;
            return base.Load();
        }

        /// <summary>
        /// Once unloaded unsubscribe from ProfileSelected events
        /// </summary>
        /// <returns></returns>
        public override bool Unload()
        {
            EventHandler.OnProfileSelected -= OnProfileSelected;
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
        /// Dynamically add every profile option to the folder
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public override IEnumerable<string> GetButtonPressActionNames(DeviceType deviceType)
        {
            var routing = new List<string>();
            foreach (var profile in GoXlrPlugin.Profiles)
            {
                routing.Add(CreateCommandName($"profile|{profile}"));
            }
            return routing;
        }

        /// <summary>
        /// Generate an Image for the profile option
        /// </summary>
        /// <param name="actionParameter"></param>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        public override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
        {
            var profileName = GetProfileName(actionParameter);
            var isSelected = GoXlrPlugin.ActiveProfile.Selected == profileName;

            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                var background = ImageHelper.GetProfileImage(
                    imageSize == PluginImageSize.Width90 ? 80 : 50,
                    profileName,
                    GoXlrConfig.Profile.Color.Font,
                    GoXlrConfig.Profile.Color.Selected,
                    GoXlrConfig.Profile.Color.NotSelected,
                    GoXlrConfig.Profile.Color.Background,
                    GoXlrConfig.Profile.Color.NotAvailable,
                    isSelected,
                    GoXlrConfig.Profile.SelectedIconPath,
                    GoXlrConfig.Profile.NotSelectedIconPath
                );

                bitmapBuilder.SetBackgroundImage(BitmapImage.FromArray(background));

                return bitmapBuilder.ToImage();
            }
        }

        /// <summary>
        /// Only Run when the GoXLR Software is connected and if the profile is available
        /// </summary>
        /// <param name="actionParameter"></param>
        public override async void RunCommand(string actionParameter)
        {
            if (Plugin.PluginStatus.Status != PluginStatus.Normal)
                return;

            var profile = GetProfileName(actionParameter);
            if (!GoXlrPlugin.Profiles.Contains(profile))
                return;

            await CommandHandler.Send(
                new GoXLR.Commands.SetProfile(profile),
                GoXlrPlugin.WsConnection
            );
        }
        
        /// <summary>
        /// Once the profile changed, invoke a refresh of the previous and selected Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProfileSelected(object sender, OnProfileSelectedEventArgs e)
        {
            CommandImageChanged($"profile|{e.Profile.Selected}");
            CommandImageChanged($"profile|{e.Profile.Previous}");
        }
        
        /// <summary>
        /// Get the profile name from actionParameter
        /// </summary>
        /// <param name="actionParameter"></param>
        /// <returns></returns>
        private static string GetProfileName(string actionParameter)
        {
            if (string.IsNullOrWhiteSpace(actionParameter))
                return string.Empty;

            return !actionParameter.Contains("|")
                ? actionParameter
                : actionParameter.Split("|")[1];
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