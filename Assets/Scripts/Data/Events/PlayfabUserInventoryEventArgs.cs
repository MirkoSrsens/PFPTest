using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when user has received user inventory data from playfab server.
    /// </summary>
    public class PlayfabUserInventoryEventArgs
    {
        /// <summary>
        /// Gets collection of item instance.
        /// </summary>
        public List<ItemInstance> ItemInstances { get; private set; }

        /// <summary>
        /// Constructor that creates new instance of <see cref="PlayfabUserInventoryEventArgs"/>.
        /// </summary>
        /// <param name="itemInstances"></param>
        public PlayfabUserInventoryEventArgs(List<ItemInstance> itemInstances)
        {
            this.ItemInstances = itemInstances;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabUserInventoryEventHandler(object sender, PlayfabUserInventoryEventArgs e);
    }
}
