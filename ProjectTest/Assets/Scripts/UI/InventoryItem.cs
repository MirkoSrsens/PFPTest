using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField]
        private Text _title;

        [SerializeField]
        private Text _numberOfUses;

        private string _itemId { get; set; }

        private void OnEnable()
        {
            if(PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshPlayerInventory += nDestroy;
            }
        }

        private void OnDisable()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshPlayerInventory -= nDestroy;
            }
        }

        public void PopulateInventoryItem(ItemInstance itemInstance)
        {
            _title.text = itemInstance.DisplayName;
            _numberOfUses.text = itemInstance.RemainingUses.HasValue ? itemInstance.RemainingUses.Value.ToString() : string.Empty;
            _itemId = itemInstance.ItemId;
        }

        public void OnClick_OpenDetails()
        {
            UIManager.Inst.OpenItemDetails(_itemId);
        }

        private void nDestroy(object sender, PlayfabUserInventoryEventArgs eventArgs)
        {
            if(sender is PlayfabManager)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
