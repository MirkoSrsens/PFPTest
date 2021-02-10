using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when user has received user readonly data from playfab server.
    /// </summary>
    public class PlayfabUserReadonlyDataEventArgs
    {
        /// <summary>
        /// Gets dictionary collection containing key value pairs of user readonly data.
        /// </summary>
        public Dictionary<string,UserDataRecord> Data { get; private set; }

        /// <summary>
        /// Constructor that creates new instance of <see cref="PlayfabUserReadonlyDataEventArgs"/>.
        /// </summary>
        /// <param name="data"></param>
        public PlayfabUserReadonlyDataEventArgs(Dictionary<string, UserDataRecord> data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabUserReadonlyDataEventHandler(object sender, PlayfabUserReadonlyDataEventArgs e);
    }
}
