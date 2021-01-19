using UnityEngine;

namespace General.State
{
    public class DesignController : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets sound controller.
        /// </summary>
        public FModSoundController soundController { get; private set; }

        /// <summary>
        /// Gets or sets animation controller.
        /// </summary>
        public  AnimationController animationController { get; private set; }

        private void Awake()
        {
            soundController = GetComponent<FModSoundController>();
            animationController = GetComponent<AnimationController>();
        }

        public void StartTask(string stateName)
        {
            if (stateName == null) return;

            if (animationController != null)
            {
                animationController.StartAnimation(stateName);
            }

            if (soundController != null)
            {
                soundController.PlaySound(stateName);
            }
        }

        public void StopTask(string stateName)
        {
            if (stateName == null) return;

            if (animationController != null)
            {
                animationController.StopAnimation(stateName);
            }

            if (soundController != null)
            {
                soundController.StopSound(stateName);
            }
        }
    }
}
