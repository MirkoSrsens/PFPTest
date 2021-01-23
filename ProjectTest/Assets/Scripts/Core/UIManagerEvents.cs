using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.Events;
using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

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
                PlayfabManager.Inst.OnRefreshCatalogItems += OnCatalogDataRecieved;
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
                PlayfabManager.Inst.OnRefreshCatalogItems -= OnCatalogDataRecieved;
                PlayfabManager.Inst.OnRefreshPlayerInventory -= OnInventoryItemsRecieved;
                PlayfabManager.Inst.OnErrorEvent -= DisplayErrorPopUp;
                PlayfabManager.Inst.OnLeaderboardRefresh -= OnLeaderboardDataRecieved;
                PlayfabManager.Inst.OnUserRegistered -= OnUserRegistered;
                PlayfabManager.Inst.OnApiCallStart -= ShowLoadingPanel;
                PlayfabManager.Inst.OnApiCallEnd -= HideLoadingPanel;
            }
        }

        /// <summary>
        /// On user info acquired setup display name.
        /// </summary>
        /// <param name="sender">The sender usually <see cref="PlayfabManager"/>.</param>
        /// <param name="eventArgs">Event data.</param>
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
        private void OnCatalogDataRecieved(object sender, PlayfabCatalogItemsEventArgs eventArgs)
        {
            foreach(var item in _catalogItemPanel.GetComponentsInChildren<Button>())
            {
                Pool.Inst.Despawn(item);
            }

            foreach (var item in eventArgs.CatalogItems)
            {
                var catalogItem = Pool.Inst.Spawn<CatalogItem>(_catalogItemPrefab, _catalogItemPanel.transform);
                var currencyOption = new List<string>(item.VirtualCurrencyPrices.Keys);

                catalogItem.Setup(item.ItemId, item.DisplayName, item.Description,
                    item.VirtualCurrencyPrices.ToString(Const.DISPLAY_CURRENCY_FORMAT), currencyOption);
            }
        }

        /// <summary>
        /// On receive items we want to display them on screen
        /// </summary>
        /// <param name="sender">The object that triggered event.</param>
        /// <param name="eventArgs">Parameters about inventory.</param>
        private void OnInventoryItemsRecieved(object sender, PlayfabUserInventoryEventArgs eventArgs)
        {
            foreach (var item in eventArgs.ItemInstances)
            {
                Pool.Inst.Spawn(_inventoryItemPrefab, _inventoryItemListPanel.transform).PopulateInventoryItem(item);
            }
        }

        /// <summary>
        /// On stats received setup them with stats UI elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void OnStatsRecieved(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            foreach (var item in eventArgs.Data)
            {
                _upgradePanel.SetStat(item.Key, item.Value.Value);
            }
        }

        /// <summary>
        /// On leaderboard received start populating it on UI.
        /// </summary>
        /// <param name="sender">Usually <see cref="PlayfabManager"/>, but can be some other object.</param>
        /// <param name="eventArgs">Data about leaderboards.</param>
        public void OnLeaderboardDataRecieved(object sender, PlayfabRefreshLeaderboardsDataEventArgs eventArgs)
        {
            for (int i = 0; i < Const.HIGHSCORE_RANGE_COUNT; i++)
            {
                if (i < eventArgs.Data.Count)
                {
                    Pool.Inst.Spawn(_leaderBoardEntityPrefab, _leaderboardListPanel.transform).Setup(eventArgs.Data[i]);
                }
                else
                {
                    var entity = Instantiate(_leaderBoardEntityPrefab, _leaderboardListPanel.transform);
                    entity.Setup(new LeaderboardData(Const.DEFAULT_LEADERBOARD_NAME, Const.DEFAULT_LEADERBOARD_SCORE, i));
                }
            }
        }

        /// <summary>
        /// Displays error on UI. Used when something with playfab goes wrong.
        /// </summary>
        /// <param name="sender">Usually <see cref="PlayfabManager"/>, but can be some other object.</param>
        /// <param name="eventArgs">Data about error message.</param>
        private void DisplayErrorPopUp(object sender, PlayfabErrorHandlingEventArgs eventArgs)
        {
            var split = eventArgs.Message.Split(Const.NEW_LINE);
            // First line is the error cloud script returned, other lines are downhill stack trace.
            _infoPanel.Setup(split[0]);
        }

        /// <summary>
        /// On user registered successfully. 
        /// </summary>
        /// <param name="sender">Usually <see cref="PlayfabManager"/>, but can be some other object.</param>
        /// <param name="eventArgs">Empty data but can contain something else in the future eg. username</param>
        private void OnUserRegistered(object sender, PlayfabOnUserRegisteredEventArgs eventArgs)
        {
            ShowLoginScreen();
            _infoPanel.Setup(Const.ON_REGISTRATION_SUCCESS);
        }

        /// <summary>
        /// On currency data refresh update main menu UI elements.
        /// </summary>
        /// <param name="sender">Usually <see cref="PlayfabManager"/>, but can be some other object.</param>
        /// <param name="eventArgs">Data about currency status and amounts.</param>
        private void OnCurrencyDataRefresh(object sender, PlayfabRefreshCurrencyEventArgs eventArgs)
        {
            _goldCount.text = eventArgs.Gold.ToString();
            _energyCount.text = eventArgs.Energy.ToString();
            _diamondCount.text = eventArgs.Diamonds.ToString();
        }
    }
}
