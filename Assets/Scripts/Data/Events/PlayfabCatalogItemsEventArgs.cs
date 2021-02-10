using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    /// <summary>
    /// Contains data and handler that will fire when new catalog item data has been received from playfab server.
    /// </summary>
    public class PlayfabCatalogItemsEventArgs : EventArgs
    {
        /// <summary>
        /// Gets list of catalog items.
        /// </summary>
        public List<CatalogItem> CatalogItems { get; private set; }

        /// <summary>
        /// Constructor that initializes new instance of <see cref="PlayfabCatalogItemsEventArgs"/>.
        /// </summary>
        /// <param name="catalogItems"></param>
        /// <param name="requestSpawning"></param>
        public PlayfabCatalogItemsEventArgs(List<CatalogItem> catalogItems)
        {
            this.CatalogItems = catalogItems;
        }

        /// <summary>
        /// Defines delegate used by event.
        /// </summary>
        /// <param name="sender">The object that fired event.</param>
        /// <param name="e">The event data.</param>
        public delegate void PlayfabCatalogItemsEventHandler(object sender, PlayfabCatalogItemsEventArgs e);
    }
}
