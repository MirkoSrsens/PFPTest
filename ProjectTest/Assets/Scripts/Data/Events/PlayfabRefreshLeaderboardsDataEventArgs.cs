using Assets.Scripts.CustomPlugins.Utility;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabRefreshLeaderboardsDataEventArgs
    {
        public List<LeaderboardData> Data { get; private set; }

        public PlayfabRefreshLeaderboardsDataEventArgs(List<LeaderboardData> data)
        {
            this.Data = data;
        }

        public delegate void PlayfabRefreshLeaderboardsDataEventHandler(object sender, PlayfabRefreshLeaderboardsDataEventArgs e);
    }
}
