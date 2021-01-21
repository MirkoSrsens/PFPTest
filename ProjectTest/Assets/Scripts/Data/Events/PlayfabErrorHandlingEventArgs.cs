using System;
using UnityEngine;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabErrorHandlingEventArgs : EventArgs
    {
        public string Message { get; private set; }

        /// <summary>
        /// In some situation we would like to jump somewhere else on the UI after error is displayed
        /// example would be on token expiration we want to jump from whatever window back to login.
        /// </summary>
        public GameObject OpenAfter { get; set; }

        public PlayfabErrorHandlingEventArgs(string message, GameObject openAfter = null)
        {
            this.Message = message;
            this.OpenAfter = openAfter;
        }

        public delegate void PlayfabErrorHandlingEventHandler(object sender, PlayfabErrorHandlingEventArgs e);
    }
}
