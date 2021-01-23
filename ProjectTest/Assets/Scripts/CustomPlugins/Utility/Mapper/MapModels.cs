namespace Assets.Scripts.CustomPlugins.Utility
{
    /// <summary>
    /// Defines leaderboard data that will be used to populate entities.
    /// </summary>
    public struct LeaderboardData
    {
        /// <summary>
        /// Gets name of user.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets user value.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets placement.
        /// </summary>
        public int Placement { get; private set ; }

        /// <summary>
        /// Defines constructor for defining <see cref="LeaderboardData"/> entity.
        /// </summary>
        /// <param name="name">The name of user.</param>
        /// <param name="value">The value of user.</param>
        /// <param name="placement">The placement of user.</param>
        public LeaderboardData(string name, int value, int placement)
        {
            // +1 because its zero based index.
            this.Placement = placement+1;
            this.Name = name;
            this.Value = value;
        }
    }

    /// <summary>
    /// Data for registration of new user.
    /// </summary>
    public struct RegisterData
    {
        /// <summary>
        /// Gets username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets password.
        /// </summary>
        public string PasswordRepeated { get; private set; }

        /// <summary>
        /// Gets email.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Defines constructor for defining <see cref="RegisterData"/> used for registration of new user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="passwordRepeated">The passowrd repeat.</param>
        public RegisterData(string username, string email, string password, string passwordRepeated)
        {
            this.Username = username;
            this.Password = password;
            this.Email = email;
            this.PasswordRepeated = passwordRepeated;
        }
    }
}
