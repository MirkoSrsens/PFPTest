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

        public void StartAnimation(string animationName)
        {
            if (Anima == null) return;
            Anima.SetBool(animationName, true);
        }

        public void StopAnimation(string animationName)
        {
            if (Anima == null) return;
            Anima.SetBool(animationName, false);
        }

        /// <summary>
        /// If on turns off if off turns on.
        /// </summary>
        /// <param name="stateAnimationName"></param>
        public void SetStateAnimation(string stateAnimationName)
        {
            if (Anima == null) return;
            if (Anima.GetBool(stateAnimationName)) Anima.SetBool(stateAnimationName, false);
            else
            {
                Anima.SetBool(stateAnimationName, true);
            }
        }

        public bool IsAnimationOver(State state, float offset = 1)
        {
            if (Anima == null) return true;

            if (Anima.GetCurrentAnimatorStateInfo(0).IsName(state.GetType().Name) && Anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= offset)
            {
                return true;
            }

            return false;
        }

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
