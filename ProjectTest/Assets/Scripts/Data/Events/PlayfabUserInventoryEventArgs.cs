using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabUserInventoryEventArgs
    {
        public List<ItemInstance> ItemInstances { get; private set; }

        public PlayfabUserInventoryEventArgs(List<ItemInstance> itemInstances)
        {
            this.ItemInstances = itemInstances;
        }

        public delegate void PlayfabUserInventoryEventHandler(object sender, PlayfabUserInventoryEventArgs e);
    }
}
