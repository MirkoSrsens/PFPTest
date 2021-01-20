using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabUserReadonlyDataEventArgs
    {
        public Dictionary<string,UserDataRecord> Data { get; private set; }

        public PlayfabUserReadonlyDataEventArgs(Dictionary<string, UserDataRecord> data)
        {
            this.Data = data;
        }

        public delegate void PlayfabUserReadonlyDataEventHandler(object sender, PlayfabUserReadonlyDataEventArgs e);
    }
}
