using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventoryItemDetails : MonoBehaviour
    {
        [SerializeField]
        private Text _title;

        [SerializeField]
        private Text _description;

        private string _itemId { get; set; }

        public void OnEnable()
        {
            PlayfabManager.Inst.OnRefreshCatalogItems += OnCatalogDataRecieved;
        }

        public void OnDisable()
        {
            PlayfabManager.Inst.OnRefreshCatalogItems -= OnCatalogDataRecieved;
        }

        public void ActivateItemDetails(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                Debug.LogError("you cannot activate details if you don't have item id to display");
                return;
            }

            _itemId = itemId;
            gameObject.SetActive(true);
        }

        public void OnClick_UseItem()
        {
            PlayfabManager.Inst.UseItem(_itemId);
            UIManager.Inst.ShowInventory();
        }

        public void OnCatalogDataRecieved(object sender, PlayfabCatalogItemsEventArgs eventArgs)
        {
            foreach(var item in eventArgs.CatalogItems)
            {
                if(item.ItemId == _itemId)
                {
                    _title.text = item.DisplayName;
                    _description.text = item.Description;
                    _itemId = item.ItemId;
                }
            }
        }
    }
}
