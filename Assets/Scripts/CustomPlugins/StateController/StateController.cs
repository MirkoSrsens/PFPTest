using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Holds function and values for controlling the states.
    /// </summary>
    public class StateController : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets identifier for data.
        /// </summary>
        public string Id;

        /// <summary>
        /// Gets and sets active state.
        /// </summary>
        public State activeState { get; set; }

        // Use this for initialization
        void Awake()
        {
            activeState = null;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (activeState != null)
            {
                activeState.WhileActiveFixed_State();
            }
        }

        private void Update()
        {
            if (activeState != null)
            {
                activeState.WhileActive_State();
            }
        }

        /// <summary>
        /// Swap state using priority.
        /// </summary>
        /// <param name="newState">The state that is attempting to perform swap.</param>
        public void SwapState(State newState)
        {
            if (newState == null) return;

            if (activeState == null || activeState.Priority < newState.Priority && activeState != newState)
            {
                ForceSwapState(newState);
            }

        }

        /// <summary>
        /// Forces swap of state. Not looking at priorities.
        /// </summary>
        /// <param name="newState">The new state.</param>
        public void ForceSwapState(State newState)
        {
            if (activeState != null) activeState.OnExit_State();
            activeState = newState;
            activeState.OnEnter_State();

        }

        /// <summary>
        /// End state.
        /// </summary>
        /// <param name="stateToEnd">The state that will be ended.</param>
        public void EndState(State stateToEnd)
        {
            if (stateToEnd == null) return;

            if (activeState == stateToEnd)
            {
                activeState.OnExit_State();
                activeState = null;
            }
        }
    }
}
