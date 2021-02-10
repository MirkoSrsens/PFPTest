using Assets.Scripts.Core;
using Assets.Scripts.CustomPlugins.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Displays chose currency panel with options to pick which currency player can pay with.
    /// </summary>
    public class ChoseCurrencyPanel : MonoBehaviour
    {
        /// <summary>
        /// Defines currency button.
        /// </summary>
        [SerializeField]
        private Button _currencyButtonPrefab;

        /// <summary>
        /// Defines parent of currency buttons.
        /// </summary>
        [SerializeField]
        private Transform _parentPanelOfCurrencyButtons;

        /// <summary>
        /// Gets or sets collection of currency buttons.
        /// </summary>
        private List<Button> currencyButtons { get; set; }

        /// <summary>
        /// Spawns buttons used to determine which currency can be used for paying.
        /// </summary>
        /// <param name="catalogItem"></param>
        public void SpawnCurrencyOptions(CatalogItem catalogItem)
        {
            currencyButtons = new List<Button>();
            foreach(var item in catalogItem.CurrencyOptions)
            {
                var button = Pool.Inst.Spawn(_currencyButtonPrefab, _parentPanelOfCurrencyButtons);
                button.GetComponentInChildren<Text>().text = item;
                button.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
                {
                    UIManager.Inst.ShowShop();
                    PlayfabManager.Inst.BuyItem(catalogItem.ItemID, item);
                }));
                currencyButtons.Add(button);
            }
        }

        // Just flush all button prefabs
        private void OnDisable()
        {
            // Cleanup of old items in shop planner section.
            for (int i = currencyButtons.Count - 1; i >= 0; i--)
            {
                currencyButtons[i].onClick.RemoveAllListeners();
                Pool.Inst.Despawn(currencyButtons[i]);
            }
        }
    }
}
