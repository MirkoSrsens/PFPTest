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

        public static RegisterData MapToModel(string username, string email, string password, string passwordConfirmation)
        {
            return new RegisterData();
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

    public struct RegisterData
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordRepeated { get; set; }

        public string Email { get; set; }

        public RegisterData(string username, string email, string password, string passwordRepeated)
        {
            this.Username = username;
            this.Password = password;
            this.Email = email;
            this.PasswordRepeated = passwordRepeated;
        }
    }
}
