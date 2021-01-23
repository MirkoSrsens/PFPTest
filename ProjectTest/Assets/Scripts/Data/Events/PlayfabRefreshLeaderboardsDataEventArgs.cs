using Assets.Scripts.CustomPlugins.Utility;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when user has received leaderboard data from playfab server.
    /// </summary>
    public class PlayfabRefreshLeaderboardsDataEventArgs
    {
        /// <summary>
        /// Gets leaderboard data.
        /// </summary>
        public List<LeaderboardData> Data { get; private set; }

        /// <summary>
        /// Constructor that creates new instance of <see cref="PlayfabRefreshLeaderboardsDataEventArgs"/>.
        /// </summary>
        /// <param name="data">The collection of leaderboard data.</param>
        public PlayfabRefreshLeaderboardsDataEventArgs(List<LeaderboardData> data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabRefreshLeaderboardsDataEventHandler(object sender, PlayfabRefreshLeaderboardsDataEventArgs e);
    }
}
