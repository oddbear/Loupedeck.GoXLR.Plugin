namespace Loupedeck.GoXLR.Plugin.GoXLR.Models
{
    public class Profile
    {
        public string Selected { get; private set; } = "";
        public string Previous { get; private set; }

        /// <summary>
        /// Set new selected profile
        /// </summary>
        /// <param name="profile">Active profile</param>
        public void SetProfile(string profile)
        {
            Previous = Selected;
            Selected = profile;
        }
    }
}