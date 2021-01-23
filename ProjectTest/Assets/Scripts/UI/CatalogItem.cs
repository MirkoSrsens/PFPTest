using Assets.Scripts.Core;
using Assets.Scripts.CustomPlugins.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Represents UI catalog item.
    /// </summary>
    public class CatalogItem : MonoBehaviour
    {
        /// <summary>
        /// Defines catalog item name.
        /// </summary>
        [SerializeField]
        private Text Name;

        /// <summary>
        /// Defines catalog item description.
        /// </summary>
        [SerializeField]
        public Text Description;

        /// <summary>
        /// Defines catalog item price.
        /// </summary>
        [SerializeField]
        public Text Price;

        /// <summary>
        /// Gets item ID.
        /// </summary>
        public string ItemID { get; private set; }

        /// <summary>
        /// Gets currency options for buying item.
        /// </summary>
        public List<string> CurrencyOptions { get; private set; }

        /// <summary>
        /// Perform setup of catalog items.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="name">The item name.</param>
        /// <param name="description">The item description.</param>
        /// <param name="price">The item price.</param>
        /// <param name="currencyOptions">The item currency options.</param>
        public void Setup(string itemId, string name, string description, string price, List<string> currencyOptions)
        {
            this.CurrencyOptions = currencyOptions;
            Name.text = name;
            Description.text = description;
            Price.text = price;
            ItemID = itemId;
        }

        /// <summary>
        /// On click display chose currency screen.
        /// </summary>
        public void OnClick_BuyItem()
        {
            UIManager.Inst.DisplayChoseValueOption(this);
        }
    }
}
