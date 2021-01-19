using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabCatalogItemsEventArgs : EventArgs
    {
        public List<CatalogItem> CatalogItems { get; private set; }

        public PlayfabCatalogItemsEventArgs(List<CatalogItem> catalogItems)
        {
            this.CatalogItems = catalogItems;
        }

        public delegate void PlayfabCatalogItemsEventHandler(object sender, PlayfabCatalogItemsEventArgs e);
    }
}
