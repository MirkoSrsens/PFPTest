using System;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabUserInfoEventArgs
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public delegate void PlayfabUserInfoEventHandler(object sender, PlayfabUserInfoEventArgs e);
    }
}
