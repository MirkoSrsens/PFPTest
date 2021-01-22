using Assets.Scripts.Data.Events;
using System.Text;

namespace Assets.Scripts.Core
{
    public partial class UIManager : SingletonBehaviour<UIManager>
    {
        private void OnEnable()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshCurrencyDataEvent += OnCurrencyDataRefresh;
                PlayfabManager.Inst.OnRefreshUserDetailsData += OnUserInfoAcquired;
                PlayfabManager.Inst.OnRefreshCatalogItems += OnCatalogItemRecieved;
                PlayfabManager.Inst.OnRefreshPlayerInventory += OnInventoryItemsRecieved;
                PlayfabManager.Inst.OnRefreshUserReadonlyData += OnStatsRecieved;
                PlayfabManager.Inst.OnErrorEvent += DisplayErrorPopUp;
                PlayfabManager.Inst.OnLeaderboardRefresh += OnLeaderboardDataRecieved;
                PlayfabManager.Inst.OnUserRegistered += OnUserRegistered;
                PlayfabManager.Inst.OnApiCallStart += ShowLoadingPanel;
                PlayfabManager.Inst.OnApiCallEnd += HideLoadingPanel;
            }
        }

        private void OnDisable()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshCurrencyDataEvent -= OnCurrencyDataRefresh;
                PlayfabManager.Inst.OnRefreshUserDetailsData -= OnUserInfoAcquired;
                PlayfabManager.Inst.OnRefreshCatalogItems -= OnCatalogItemRecieved;
                PlayfabManager.Inst.OnRefreshPlayerInventory -= OnInventoryItemsRecieved;
                PlayfabManager.Inst.OnErrorEvent -= DisplayErrorPopUp;
                PlayfabManager.Inst.OnLeaderboardRefresh -= OnLeaderboardDataRecieved;
                PlayfabManager.Inst.OnUserRegistered -= OnUserRegistered;
                PlayfabManager.Inst.OnApiCallStart -= ShowLoadingPanel;
                PlayfabManager.Inst.OnApiCallEnd -= HideLoadingPanel;
            }
        }

        private void OnUserInfoAcquired(object sender, PlayfabUserInfoEventArgs eventArgs)
        {
            _usernameDisplay.text = eventArgs.Username;
        }

        /// <summary>
        /// Kinda mixed feelings where to put this method. It spawns UI elements but Catalog item objects
        /// should be handled by server. There is cloud script function ButItem that will prevent user from
        /// messing with this.
        /// </summary>
        /// <param name="sender">Playfab manager usually but can be subscribed somewhere else in the future</param>
        /// <param name="eventArgs">Data about items.</param>
        private void OnCatalogItemRecieved(object sender, PlayfabCatalogItemsEventArgs eventArgs)
        {
            // Cleanup of old items in shop planner section.
            for (int i = _catalogItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_catalogItemPanel.transform.GetChild(i).gameObject);
            }

            foreach (var item in eventArgs.CatalogItems)
            {
                var catalogItem = Instantiate(_catalogItemPrefab, _catalogItemPanel.transform);
                catalogItem.Name.text = item.DisplayName;
                catalogItem.Description.text = item.Description;

                StringBuilder sb = new StringBuilder();
                foreach (var currency in item.VirtualCurrencyPrices)
                {
                    sb.Append(string.Concat(currency.Key, ":", currency.Value, "/"));
                    catalogItem.currencyOptions.Add(currency.Key);
                }
                catalogItem.ItemID = item.ItemId;
                catalogItem.Price.text = sb.ToString();
            }
        }

        /// <summary>
        /// When we receive items we want to display them on screen
        /// </summary>
        /// <param name="sender">The object that triggered event.</param>
        /// <param name="eventArgs">Parameters about inventory.</param>
        private void OnInventoryItemsRecieved(object sender, PlayfabUserInventoryEventArgs eventArgs)
        {
            foreach (var item in eventArgs.ItemInstances)
            {
                var inventoryItem = Instantiate(_inventoryItemPrefab, _inventoryItemListPanel.transform);
                inventoryItem.PopulateInventoryItem(item);
            }
        }

        public void OnStatsRecieved(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            foreach (var item in eventArgs.Data)
            {
                _upgradePanel.SetStat(item.Key, item.Value.Value);
            }
        }

        public void OnLeaderboardDataRecieved(object sender, PlayfabRefreshLeaderboardsDataEventArgs eventArgs)
        {
            const int minimumNumberOfPlaces = 10;
            var current = 0;

            foreach (var item in eventArgs.Data)
            {
                var entity = Instantiate(_leaderBoardEntityPrefab, _leaderboardListPanel.transform);
                entity.Setup(item);
                current++;
            }

            for (int i = current; i < minimumNumberOfPlaces; i++)
            {
                var entity = Instantiate(_leaderBoardEntityPrefab, _leaderboardListPanel.transform);
                entity.Setup(new CustomPlugins.Utility.LeaderboardData("-", 0, i));
            }
        }

        private void DisplayErrorPopUp(object sender, PlayfabErrorHandlingEventArgs eventArgs)
        {
            var split = eventArgs.Message.Split('\n');
            _infoPanel.Setup(split[0]);
        }

        private void OnUserRegistered(object sender, PlayfabOnUserRegisteredEventArgs eventArgs)
        {
            ShowLoginScreen();
            _infoPanel.Setup("New user registered, please login!");
        }

        private void OnCurrencyDataRefresh(object sender, PlayfabRefreshCurrencyEventArgs eventArgs)
        {
            _goldCount.text = eventArgs.Gold.ToString();
            _energyCount.text = eventArgs.Energy.ToString();
            _diamondCount.text = eventArgs.Diamonds.ToString();
        }
    }
}
