using System;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire before playfab api has been called.
    /// </summary>
    public class PlayfabOnApiCallStartEventArgs : EventArgs
    {
        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabOnApiCallStartEventHandler();
    }
}
