using System.Collections;
using UnityEngine;

namespace General.State
{
    public class AnimationController:MonoBehaviour
    {
        /// <summary>
        /// Gets or sets animator; 
        /// </summary>
        public Animator Anima { get; set; }

        private void Awake()
        {
            Anima = GetComponentInChildren<Animator>();
        }

        /// <summary>
        /// Starts animation sequence
        /// </summary>
        /// <param name="animationName">The name of animation.</param>
        /// <param name="useTrigger">Value indicating weather trigger should be used or flag.</param>
        public void StartAnimation(string animationName, bool useTrigger = false)
        {
            if (Anima == null) return;

            if (useTrigger)
            {
                Anima.SetTrigger(animationName);
            }
            else
            {
                Anima.SetBool(animationName, true);
            }
        }

        /// <summary>
        /// Stops animation with name. Only uses with bool animations.
        /// </summary>
        /// <param name="animationName">Name of animation.</param>
        public void StopAnimation(string animationName)
        {
            if (Anima == null) return;
            Anima.SetBool(animationName, false);
        }

        /// <summary>
        /// If on turns off if off turns on.
        /// </summary>
        /// <param name="stateAnimationName">Name of animation</param>
        public void SetStateAnimation(string stateAnimationName)
        {
            if (Anima == null) return;
            if (Anima.GetBool(stateAnimationName)) Anima.SetBool(stateAnimationName, false);
            else
            {
                Anima.SetBool(stateAnimationName, true);
            }
        }

        /// <summary>
        /// Checks if animation is over specific % of executing. If animation event is not an option.
        /// </summary>
        /// <param name="state">State for which we are expecting animation.</param>
        /// <param name="offset">Offset for which we are expecting animation.</param>
        /// <returns>Value indicating weather animation is over specific % or not.</returns>
        public bool IsAnimationOver(State state, float offset = 1)
        {
            if (Anima == null) return true;

            if (Anima.GetCurrentAnimatorStateInfo(0).IsName(state.GetType().Name) && Anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= offset)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if animation is over specific % of executing. If animation event is not an option.
        /// </summary>
        /// <param name="animationName">name of animation for which we are expecting animation.</param>
        /// <param name="offset">Offset for which we are expecting animation.</param>
        /// <returns>Value indicating weather animation is over specific % or not.</returns>
        public bool IsAnimationOver(string animationName, float offset = 1)
        {
            if (Anima == null) return true;

            if (Anima.GetCurrentAnimatorStateInfo(0).IsName(animationName) && Anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= offset)
            {
                return true;
            }

            return false;
        }
    }
}
