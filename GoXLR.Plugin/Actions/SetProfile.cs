using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loupedeck.GoXLR.Plugin.Config;
using Loupedeck.GoXLR.Plugin.GoXLR;
using Loupedeck.GoXLR.Plugin.GoXLR.EventArgs;
using Loupedeck.GoXLR.Plugin.Helpers;
using EventHandler = Loupedeck.GoXLR.Plugin.GoXLR.EventHandler;

namespace Loupedeck.GoXLR.Plugin.Actions
{
    public class SetProfile : MultistateActionEditorCommand
    {
        private const string ProfileControlName = "profile";
        private const int Selected = 0;
        private const int NotSelected = 1;

        public SetProfile()
        {
            DisplayName = "Profile";
            Description = "Select the Profile of your GoXLR-Device";
            GroupName = "";

            AddState("Selected", "The profile that is currently selected");
            AddState("Not Selected", "The profile that isn't currently selected");

            ActionEditor.AddControl(new ActionEditorListbox(ProfileControlName, "Profile:").SetRequired());
            ActionEditor.ListboxItemsRequested += OnProfilesChanged;
        }

        /// <summary>
        /// Add the Items to the List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnProfilesChanged(object sender, ActionEditorListboxItemsRequestedEventArgs e)
        {
            if (!e.ControlName.EqualsNoCase(ProfileControlName)) return;

            foreach (var profile in GoXlrPlugin.Profiles)
            {
                e.AddItem(profile, profile, "");
            }
        }

        /// <summary>
        /// Once loaded subscribe to ProfileSelected events
        /// </summary>
        /// <returns></returns>
        protected override bool OnLoad()
        {
            EventHandler.OnProfileSelected += OnRefreshIcons;
            EventHandler.RefreshIcons += OnRefreshIcons;
            return true;
        }

        /// <summary>
        /// Once loaded subscribe to ProfileSelected events
        /// </summary>
        /// <returns></returns>
        protected override bool OnUnload()
        {
            EventHandler.OnProfileSelected -= OnRefreshIcons;
            EventHandler.RefreshIcons -= OnRefreshIcons;
            return true;
        }

        /// <summary>
        /// Generate an Image for the profile option
        /// </summary>
        /// <param name="actionParameters"></param>
        /// <param name="stateIndex"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <returns></returns>
        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, int stateIndex, int imageWidth,
            int imageHeight)
        {
            var profileName = actionParameters.Parameters[ProfileControlName];
            bool? isSelected = GoXlrPlugin.ActiveProfile.Selected == profileName;

            if (Plugin.PluginStatus.Status != PluginStatus.Normal)
                isSelected = null;

            using (var bitmapBuilder = new BitmapBuilder(imageWidth == 80 ? PluginImageSize.Width90 : PluginImageSize.Width60))
            {
                byte[] background;
                try
                {
                    background = ImageHelper.GetProfileImage(
                        imageWidth,
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
                }
                catch (Exception e)
                {
                    Plugin.Log.Error(e, "Error");
                    throw;
                }

                bitmapBuilder.SetBackgroundImage(BitmapImage.FromArray(background));

                return bitmapBuilder.ToImage();
            }
        }

        /// <summary>
        ///  Only Run when the GoXLR Software is connected and if the profile is available
        /// </summary>
        /// <param name="actionParameters"></param>
        protected override bool RunCommand(ActionEditorActionParameters actionParameters)
        {
            return Task.Run(async () =>
            {
                if (Plugin.PluginStatus.Status != PluginStatus.Normal)
                    return false;

                var profile = actionParameters.Parameters[ProfileControlName];
                if (!GoXlrPlugin.Profiles.Contains(profile))
                    return false;

                await CommandHandler.Send(
                    new GoXLR.Commands.SetProfile(profile),
                    GoXlrPlugin.WsConnection
                );

                return true;
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Once the profile changed, invoke a refresh of the previous and selected Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnRefreshIcons(object sender, EventArgs eventArgs)
        {
            if (eventArgs.GetType() == typeof(OnProfileSelectedEventArgs))
            {
                var args = (OnProfileSelectedEventArgs) eventArgs;
                SetCurrentState(
                    new ActionEditorActionParameters(
                        new Dictionary<string, string>
                        {
                            [ProfileControlName] = args.Profile.Selected
                        }), Selected);

                SetCurrentState(
                    new ActionEditorActionParameters(
                        new Dictionary<string, string>
                        {
                            [ProfileControlName] = args.Profile.Previous
                        }), NotSelected);
            }

            ActionImageChanged();
        }
    }
}