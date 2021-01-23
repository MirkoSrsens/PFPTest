using System;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when user has been registered successfully.
    /// </summary>
    public class PlayfabOnUserRegisteredEventArgs : EventArgs
    {
        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabOnUserRegisteredEventHandler(object sender, PlayfabOnUserRegisteredEventArgs e);
    }
}
