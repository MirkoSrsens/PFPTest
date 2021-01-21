using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.Scripts.CustomPlugins.Utility
{
    public static class Mappings
    {
        public static List<LeaderboardData> MapToModel(this List<PlayerLeaderboardEntry> data)
        {
            var result = new List<LeaderboardData>();

            foreach(var item in data)
            {
                var name = string.IsNullOrEmpty(item.DisplayName) ? item.PlayFabId : item.DisplayName;
                result.Add(new LeaderboardData(name, item.StatValue, item.Position));
            }

            return result;
        }
    }

    // Map models
    public struct LeaderboardData
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public int Placement { get; set; }

        public LeaderboardData(string name, int value, int placement)
        {
            this.Placement = placement;
            this.Name = name;
            this.Value = value;
        }
    }
}
