using Assets.Scripts.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CatalogItem : MonoBehaviour
    {
        public Text Name;

        public Text Description;

        public Text Price;

        public string ItemID { get; set; }

        public List<string> currencyOptions { get; set; } = new List<string>();

        public void OnClick_BuyItem()
        {
            UIManager.Inst.DisplayChoseValueOption(this);
        }
    }
}
