using Assets.Scripts.Data.Events;
using Assets.Scripts.Data.Scriptable;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [Header("Intro data")]
        [SerializeField]
        private IntroSequence _introSequenceData;

        [SerializeField]
        private GameObject _introPanel;

        [SerializeField]
        private float _alphaChangeSpeed;

        [Header("Playfab Data")]

        [SerializeField]
        private GameObject _loginPanel;

        [SerializeField]
        private GameObject _genericErrorBox;

        [Header("Login elements")]
        [SerializeField]
        private InputField _username;

        [SerializeField]
        private InputField _password;

        [Header("MainMenu elements")]
        [SerializeField]
        private GameObject _mainMenuPanel;

        [SerializeField]
        private Text _usernameDisplay;

        [SerializeField]
        // Soft currency
        private Text _goldCount;

        [SerializeField]
        private Text _energyCount;

        [SerializeField]
        // Hard currency
        private Text _diamondCount;

        [Header("Shop elements")]
        [SerializeField]
        private GameObject _shopPanel;

        [SerializeField]
        private GameObject _catalogItemPanel;

        [SerializeField]
        private CatalogItem _catalogItemPrefab;

        [SerializeField]
        private ChoseCurrencyPanel _choseCurrencyPanel;


        [Header("InventoryItems elements")]
        [SerializeField]
        private GameObject _inventoryPanel;

        [SerializeField]
        private GameObject _inventoryItemListPanel;

        [SerializeField]
        private InventoryItem _inventoryItemPrefab;

        [SerializeField]
        private InventoryItemDetails _inventoryItemDetails;


        private void OnEnable()
        {
            PlayfabManager.Inst.RefreshCurrencyDataEvent += OnCurrencyDataRefresh;
            PlayfabManager.Inst.RefreshUserDetailsData += OnUserInfoAcquired;
            PlayfabManager.Inst.RefreshCatalogItems += OnCatalogItemRecieved;
            PlayfabManager.Inst.RefreshPlayerInventory += OnInventoryItemsRecieved;
        }

        private void OnDisable()
        {
            PlayfabManager.Inst.RefreshCurrencyDataEvent -= OnCurrencyDataRefresh;
            PlayfabManager.Inst.RefreshUserDetailsData -= OnUserInfoAcquired;
            PlayfabManager.Inst.RefreshCatalogItems -= OnCatalogItemRecieved;
            PlayfabManager.Inst.RefreshPlayerInventory -= OnInventoryItemsRecieved;
        }

        public IEnumerator PlayIntroSequence()
        {
            CloseAllExcept(_introPanel);

            foreach (var item in _introSequenceData.SequenceOfObjects)
            {
                var image = Instantiate(item.SplashImage, _introPanel.transform).GetComponent<Image>();
                image.ChangeAlpha(0);
                while(image.color.a < 0.99f)
                {
                    image.ChangeAlpha(image.color.a + _alphaChangeSpeed * Time.deltaTime);
                    // yeh you can predefine this but its intro sequence.
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(item.Duration);

                Destroy(image.gameObject);
            }

            _introPanel.SetActive(false);
        }

        /// Maybe in the future other logic will go in <see cref="ShowLoginScreen"/> and <see cref="CloseLoginScreen"/>.
        public void ShowLoginScreen()
        {
            CloseAllExcept(_loginPanel);
        }

        public void ShowMainMenu()
        {
            CloseAllExcept(_mainMenuPanel);
        }

        public void ShowShop(bool refreshCatalog = true)
        {
            if (refreshCatalog)
            {
                PlayfabManager.Inst.GetCatalogItems();
            }
            CloseAllExcept(_shopPanel);
        }

        public void ShowInventory(bool refreshInventory = true)
        {
            if (refreshInventory)
            {
                PlayfabManager.Inst.GetUserInventory();
            }
            CloseAllExcept(_inventoryPanel);
        }

        private void CloseAllExcept(GameObject panel = null)
        {
            //Careful this will trigger on enable/disable for activating element.
            _introPanel.SetActive(false);
            _loginPanel.SetActive(false);
            _mainMenuPanel.SetActive(false);
            _shopPanel.SetActive(false);
            _choseCurrencyPanel.gameObject.SetActive(false);
            _inventoryPanel.SetActive(false);
            _inventoryItemDetails.gameObject.SetActive(false);

            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        public void OnPlayfabLoginClicked()
        {
            PlayfabManager.Inst.PerformLogin(_username.text, _password.text, 
                success =>
                {
                    GameManager.Inst.StartMainMenuState();
                },
                failed =>
                {
                    DisplayGenericPlayfabError(PlayfabManager.Inst.GenericMessage(failed));
                });
        }

        private void OnCurrencyDataRefresh(object sender, PlayfabRefreshCurrencyEventArgs eventArgs)
        {
            _goldCount.text = eventArgs.Gold.ToString();
            _energyCount.text = eventArgs.Energy.ToString();
            _diamondCount.text = eventArgs.Diamonds.ToString();
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
            for(int i = _catalogItemPanel.transform.childCount -1; i>= 0; i--)
            {
                Destroy(_catalogItemPanel.transform.GetChild(i).gameObject);
            }

            foreach(var item in eventArgs.CatalogItems)
            {
                var catalogItem = Instantiate(_catalogItemPrefab, _catalogItemPanel.transform);
                catalogItem.Name.text = item.DisplayName;
                catalogItem.Description.text = item.Description;

                StringBuilder sb = new StringBuilder();
                foreach(var currency in item.VirtualCurrencyPrices)
                {
                    sb.Append(string.Concat(currency.Key, ":", currency.Value, "/"));
                    catalogItem.currencyOptions.Add(currency.Key);
                }
                catalogItem.ItemID = item.ItemId;
                catalogItem.Price.text = sb.ToString();
            }
        }

        public void DisplayChoseValueOption(CatalogItem catalogItem)
        {
            // We dont want to close shop panel in background just for aesthetics reasons :D.
            _choseCurrencyPanel.gameObject.SetActive(true);
            _choseCurrencyPanel.SpawnCurrencyOptions(catalogItem);
        }

        /// <summary>
        /// When we receive items we want to display them on screen
        /// </summary>
        /// <param name="sender">The object that triggered event.</param>
        /// <param name="eventArgs">Parameters about inventory.</param>
        private void OnInventoryItemsRecieved(object sender, PlayfabUserInventoryEventArgs eventArgs)
        {
            // Cleanup of old items in shop planner section.
            for (int i = _inventoryItemListPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_inventoryItemListPanel.transform.GetChild(i).gameObject);
            }

            foreach (var item in eventArgs.ItemInstances)
            {
                var inventoryItem = Instantiate(_inventoryItemPrefab, _inventoryItemListPanel.transform);
                inventoryItem.PopulateInventoryItem(item);
            }
        }

        public void OpenItemDetails(string itemId)
        {
            _inventoryItemDetails.ActivateItemDetails(itemId);
            PlayfabManager.Inst.GetCatalogItems();
        }

        public void DisplayGenericPlayfabError(string message)
        {
        }

    }
}
