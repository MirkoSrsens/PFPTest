﻿using UnityEngine;

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

        /// <summary>
        /// Starts visual and audio effects of state.
        /// </summary>
        /// <param name="stateName">Name of state.</param>
        /// <param name="useTrigger">Should trigger be used or not.</param>
        public void StartTask(string stateName, bool useTrigger = false)
        {
            if (stateName == null) return;

            if (animationController != null)
            {
                animationController.StartAnimation(stateName, useTrigger);
            }

            if (soundController != null)
            {
                soundController.PlaySound(stateName);
            }
        }

        /// <summary>
        /// Stops visual and audio effects of state.
        /// </summary>
        /// <param name="stateName">Name of state.</param>
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
