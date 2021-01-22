using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UpgradePanel : MonoBehaviour
    {
        private StatButton[] _statButtons { get; set; }

        private void Awake()
        {
            _statButtons = GetComponentsInChildren<StatButton>();
        }

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
