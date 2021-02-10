using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Defines inventory item details used to display additional info once item is clicked.
    /// </summary>
    public class InventoryItemDetails : MonoBehaviour
    {
        /// <summary>
        /// Defines title that contains name of the item.
        /// </summary>
        [SerializeField]
        private Text _title;

        /// <summary>
        /// Defines description of the item
        /// </summary>
        [SerializeField]
        private Text _description;

        /// <summary>
        /// Gets or sets item identifier.
        /// </summary>
        private string _itemId { get; set; }

        public void OnEnable()
        {
            PlayfabManager.Inst.OnRefreshCatalogItems += OnCatalogDataRecieved;
        }

        public void OnDisable()
        {
            PlayfabManager.Inst.OnRefreshCatalogItems -= OnCatalogDataRecieved;
        }

        /// <summary>
        /// Calls activation of the item.
        /// </summary>
        /// <param name="itemId">The item unique identifier.</param>
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

        /// <summary>
        /// On click perform usage of item 
        /// </summary>
        public void OnClick_UseItem()
        {
            PlayfabManager.Inst.UseItem(_itemId);
            UIManager.Inst.ShowInventory();
        }

        /// <summary>
        /// On catalog data received populate inventory details.
        /// </summary>
        /// <param name="sender">The sender. Usually <see cref="InventoryItemDetails"/>.</param>
        /// <param name="eventArgs">The event arguments.</param>
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
