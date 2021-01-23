using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Used for error displaying and other useful information.
    /// </summary>
    public class InfoPanel : MonoBehaviour
    {
        /// <summary>
        /// Defines text component that will contain message.
        /// </summary>
        [SerializeField]
        private Text _text;

        /// <summary>
        /// Defines close button component.
        /// </summary>
        [SerializeField]
        private Button _closeButton;

        /// <summary>
        /// Setups info panel with proper message.
        /// </summary>
        /// <param name="message">Message to display.</param>
        public void Setup(string message)
        {
            gameObject.SetActive(true);
            _text.text = message;
        }
    }
}
