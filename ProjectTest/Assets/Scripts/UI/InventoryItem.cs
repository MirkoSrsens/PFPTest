using Assets.Scripts.Core;
using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.Events;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Defines single inventory item.
    /// </summary>
    public class InventoryItem : MonoBehaviour
    {
        /// <summary>
        /// Defines title of item.
        /// </summary>
        [SerializeField]
        private Text _title;

        /// <summary>
        /// Defines texts used to display remaining number of uses.
        /// </summary>
        [SerializeField]
        private Text _numberOfUses;

        /// <summary>
        /// Defines item unique id.
        /// </summary>
        private string _itemId { get; set; }

        private void OnEnable()
        {
            if(PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshPlayerInventory += Despawn;
            }
        }

        private void OnDisable()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshPlayerInventory -= Despawn;
            }
        }

        /// <summary>
        /// Populates inventory item with data.
        /// </summary>
        /// <param name="itemInstance">The item data.</param>
        public void PopulateInventoryItem(ItemInstance itemInstance)
        {
            _title.text = itemInstance.DisplayName;
            _numberOfUses.text = itemInstance.RemainingUses.HasValue ? itemInstance.RemainingUses.Value.ToString() : string.Empty;
            _itemId = itemInstance.ItemId;
        }

        /// <summary>
        /// Opens item details.
        /// </summary>
        public void OnClick_OpenDetails()
        {
            UIManager.Inst.OpenItemDetails(_itemId);
        }

        /// <summary>
        /// De initializes item on refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void Despawn(object sender, PlayfabUserInventoryEventArgs eventArgs)
        {
            if(sender is PlayfabManager)
            {
                if (Pool.Inst != null)
                {
                    Pool.Inst.Despawn(this);
                }
            }
        }
    }
}
