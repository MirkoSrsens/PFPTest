using PlayFab;
using PlayFab.ClientModels;
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
        public PlayFabError PlayfabError { get; private set; }

        /// <summary>
        /// Gets message defining script execution error.
        /// </summary>
        public ScriptExecutionError ScriptExecutionError { get; private set; }

        /// <summary>
        /// Constructor that creates new instance of <see cref="PlayfabErrorHandlingEventArgs"/>.
        /// </summary>
        /// <param name="message">The message that will be used.</param>
        /// <param name="errorCode">The error code that will be used.</param>
        public PlayfabErrorHandlingEventArgs(PlayFabError error)
        {
            this.PlayfabError = error;
        }

        /// <summary>
        /// Constructor that creates new instance of <see cref="PlayfabErrorHandlingEventArgs"/>.
        /// </summary>
        /// <param name="message">The message that will be used.</param>
        /// <param name="errorCode">The error code that will be used.</param>
        public PlayfabErrorHandlingEventArgs(ScriptExecutionError error)
        {
            this.ScriptExecutionError = error;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabErrorHandlingEventHandler(object sender, PlayfabErrorHandlingEventArgs eventArgs);
    }
}
