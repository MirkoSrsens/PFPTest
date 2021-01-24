using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Defines types of stats that can be upgraded.
    /// There is lock on cloud preventing different stats from being posted.
    /// </summary>
    public enum StatType
    {
        WingFlapsStrenght = 0, // Wing flaps strength
        MovementSpeed = 1, // Movement speed
        GoldMultiplier = 2, // Gold multiplier
    }
    public class StatButton : MonoBehaviour
    {
        /// <summary>
        /// The type of stat.
        /// </summary>
        public StatType Type;

        /// <summary>
        /// Gets or sets number of current upgrades for UI display.
        /// </summary>
        private Text _numberOfUpgrades { get; set; }

        /// <summary>
        /// Gets or sets numberic value of current value.
        /// </summary>
        private int currentLevel { get; set; }

        private void Awake()
        {
            _numberOfUpgrades = GetComponentInChildren<Text>();
        }
        
        /// <summary>
        /// Used to setup stats values and parse ui display.
        /// </summary>
        /// <param name="level"></param>
        public void SetStat(string level)
        {
            var number = int.Parse(level);
            var cost = number.CalculateCost().ToString();
            _numberOfUpgrades.text = string.Format(Const.FORMAT_STATS_TEXT, level, cost);
            currentLevel = number;
        }

        /// <summary>
        /// On click try to perform upgrade of stats.
        /// </summary>
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
