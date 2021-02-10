using System;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire after playfab api has returned response.
    /// </summary>
    public class PlayfabOnApiCallEndEventArgs : EventArgs
    {
        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        public delegate void PlayfabOnApiCallEndEventHandler();
    }
}
