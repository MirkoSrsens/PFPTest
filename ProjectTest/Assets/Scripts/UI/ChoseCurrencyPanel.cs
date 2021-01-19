using Assets.Scripts.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ChoseCurrencyPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _buttonPrefab;

        [SerializeField]
        private Transform _buttonsPanel;

        public void SpawnCurrencyOptions(CatalogItem catalogItem)
        {
            foreach(var item in catalogItem.currencyOptions)
            {
                var button = Instantiate(_buttonPrefab, _buttonsPanel);
                button.GetComponentInChildren<Text>().text = item;
                button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => PlayfabManager.Inst.BuyItem(catalogItem.ItemID, item)));
            }
        }

        // Just flush all button prefabs
        private void OnDisable()
        {
            // Cleanup of old items in shop planner section.
            for (int i = _buttonsPanel.childCount - 1; i >= 0; i--)
            {
                Destroy(_buttonsPanel.GetChild(i).gameObject);
            }
        }
    }
}
