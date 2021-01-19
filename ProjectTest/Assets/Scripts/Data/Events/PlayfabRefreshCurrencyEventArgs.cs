using System;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabRefreshCurrencyEventArgs : EventArgs
    {
        public int Gold { get; private set; }

        public int Energy { get; private set; }

        public int Diamonds { get; private set; }

        public PlayfabRefreshCurrencyEventArgs(int energy, int gold, int diamonds)
        {
            this.Gold = gold;
            this.Energy = energy;
            this.Diamonds = diamonds;
        }

        public delegate void PlayfabRefreshCurrencyEventHandler(object sender, PlayfabRefreshCurrencyEventArgs e);
    }
}
