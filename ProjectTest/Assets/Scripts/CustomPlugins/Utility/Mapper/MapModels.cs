namespace Assets.Scripts.CustomPlugins.Utility
{
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
