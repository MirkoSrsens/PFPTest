using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Defines upgrade panel used for upgrading stats.
    /// </summary>
    public class UpgradePanel : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets stat buttons used for upgrading of stats.
        /// </summary>
        private StatButton[] _statButtons { get; set; }

        private void Awake()
        {
            _statButtons = GetComponentsInChildren<StatButton>();
        }

        /// <summary>
        /// Sets stats value depening on provided key.
        /// </summary>
        /// <param name="key">The key value representing stat.</param>
        /// <param name="value">The value representing value of specific stat.</param>
        public void SetStat(string key, string value)
        {
            if(_statButtons == null)
            {
                return;
            }

            foreach(var button in _statButtons)
            {
                // Can be done with numbers or some other values
                // but makes it easier to debug and figure out whats happening.
                if(button.Type.ToString() == key)
                {
                    button.SetStat(value);
                    return;
                }
            }
        }

    }
}
