using System;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabOnUserRegisteredEventArgs : EventArgs
    {
        public delegate void PlayfabOnUserRegisteredEventHandler(object sender, PlayfabOnUserRegisteredEventArgs e);
    }
}
