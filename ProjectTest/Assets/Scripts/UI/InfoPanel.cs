using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        [SerializeField]
        private Button _closeButton;

        public void Setup(string message, GameObject openAfter = null)
        {
            gameObject.SetActive(true);
            _text.text = message;
            _closeButton.onClick.AddListener(new UnityEngine.Events.UnityAction( 
                () => 
                {
                    if (UIManager.Inst != null && openAfter != null)
                    {
                        UIManager.Inst.CloseAllExcept(openAfter);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }));
        }
    }
}
