using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public enum StatType
    {
        Health = 0,
        Speed = 1,
        GoldMultiplier = 2,
    }
    public class StatButton : MonoBehaviour
    {
        public StatType Type;

        private Text _numberOfUpgrades { get; set; }

        private int currentLevel { get; set; }

        private void Awake()
        {
            _numberOfUpgrades = GetComponentInChildren<Text>();
        }

        public void SetStat(string level)
        {
            var number = int.Parse(level);
            var cost = number.CalculateCost().ToString();
            _numberOfUpgrades.text = string.Concat(level,"/", cost);
            currentLevel = number;
        }

        public void OnClick_UpgradeStat()
        {
            PlayfabManager.Inst.UpgradeStat(Type.ToString(),
                () =>
                {
                    ++currentLevel;
                    SetStat(currentLevel.ToString());
                });
        }
    }
}
