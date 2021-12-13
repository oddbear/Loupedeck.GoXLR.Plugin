﻿namespace Loupedeck.GoXLRPlugin.Commands
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading;

    using GoXLR.Server.Models;

    using Helpers;

    class SetProfileCommand : PluginDynamicCommand
    {
        private GoXLRPlugin _plugin;

        private Profile[] _profiles = Array.Empty<Profile>();
        private Profile _selected = Profile.Empty;

        public SetProfileCommand() : base()
        {
            this.DisplayName = "Profile Set";
            this.GroupName = "";
            this.Description = "Select profile.";

            this.MakeProfileAction("list;Profile:");
        }

        protected override Boolean OnLoad()
        {
            this._plugin = (GoXLRPlugin)base.Plugin;
            return true;
        }

        public void UpdateProfileList(Profile[] profiles) =>
            this._profiles = profiles;

        public void UpdateSelectedProfile(Profile profile)
        {
            var prevProfile = this._selected;
            this._selected = profile;

            if (this._selected != prevProfile)
            {
                var profileName = profile.Name;
                this.ActionImageChanged($"profile|{profileName}");
            }
        }

        protected override void RunCommand(String actionParameter)
        {
            if (actionParameter.IndexOf('|') == -1)
                return;

            var split = actionParameter.Split('|');
            var profileName = split[1];
            var profile = new Profile(profileName);

            var server = this._plugin.Server;
            server.SetProfile(profile, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        protected override PluginActionParameter[] GetParameters() =>
            this._profiles
                .Select(profile => profile.Name)
                .Select(profileName => new PluginActionParameter($"profile|{profileName}", profileName, String.Empty))
                .ToArray();

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                var background = ImageHelpers.GetImageBackground(imageSize, Color.Red);
                var bitmapImage = new BitmapImage(background);
                bitmapBuilder.SetBackgroundImage(bitmapImage);
                
                var text = this.GetCommandDisplayName(actionParameter, imageSize);
                bitmapBuilder.DrawText(text);

                return bitmapBuilder.ToImage();
            }
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter is null)
                return String.Empty;

            var text = actionParameter;
            if (actionParameter.IndexOf('|') != -1)
            {
                var split = actionParameter.Split('|');
                text = split[1];
            }

            return text == this._selected.Name
                ? $"[{text}]"
                : text;
        }
    }
}