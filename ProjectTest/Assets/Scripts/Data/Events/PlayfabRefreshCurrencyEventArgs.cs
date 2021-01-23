using System;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when user has received currency values from playfab server.
    /// </summary>
    public class PlayfabRefreshCurrencyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets amount of gold user currently posses. 
        /// </summary>
        public int Gold { get; private set; }

        /// <summary>
        /// Gets amount of energy user currently posses. 
        /// </summary>
        public int Energy { get; private set; }

        /// <summary>
        /// Gets amount of diamonds user currently posses. 
        /// </summary>
        public int Diamonds { get; private set; }

        /// <summary>
        /// Constructor user for creating new instance of <see cref="PlayfabRefreshCurrencyEventArgs"/>
        /// </summary>
        /// <param name="energy">The amount of energy.</param>
        /// <param name="gold">The amount of gold.</param>
        /// <param name="diamonds">The amount of gold.</param>
        public PlayfabRefreshCurrencyEventArgs(int energy, int gold, int diamonds)
        {
            this.Gold = gold;
            this.Energy = energy;
            this.Diamonds = diamonds;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabRefreshCurrencyEventHandler(object sender, PlayfabRefreshCurrencyEventArgs e);
    }
}
