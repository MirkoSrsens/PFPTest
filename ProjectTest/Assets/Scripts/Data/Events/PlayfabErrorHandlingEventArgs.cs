using System;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabErrorHandlingEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public PlayfabErrorHandlingEventArgs(string message)
        {
            this.Message = message;
        }

        public delegate void PlayfabErrorHandlingEventHandler(object sender, PlayfabErrorHandlingEventArgs e);
    }
}
