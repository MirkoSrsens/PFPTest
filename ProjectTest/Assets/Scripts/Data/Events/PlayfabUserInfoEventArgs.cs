namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when user has received user info from playfab server.
    /// </summary>
    public class PlayfabUserInfoEventArgs
    {
        /// <summary>
        /// Gets username value.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets email value.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Constructor that creates instance of <see cref="PlayfabUserInfoEventArgs"/>.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        public PlayfabUserInfoEventArgs(string username, string email)
        {
            Username = username;
            Email = email;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabUserInfoEventHandler(object sender, PlayfabUserInfoEventArgs e);
    }
}
