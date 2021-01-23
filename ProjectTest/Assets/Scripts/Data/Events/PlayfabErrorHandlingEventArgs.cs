using System;
using UnityEngine;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when error data has been received from playfab server.
    /// </summary>
    public class PlayfabErrorHandlingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets message defining error that happened.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Constructor that creates new instance of <see cref="PlayfabErrorHandlingEventArgs"/>.
        /// </summary>
        /// <param name="message">The message that will be used.</param>
        public PlayfabErrorHandlingEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabErrorHandlingEventHandler(object sender, PlayfabErrorHandlingEventArgs eventArgs);
    }
}
