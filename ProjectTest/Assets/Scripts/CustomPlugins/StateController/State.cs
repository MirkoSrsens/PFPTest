using System;
using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Core state for all actions.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected State(StateManager stateManager)
        {
            this.stateManager = stateManager;
            DiContainerLibrary.DiContainer.DiContainerInitializor.RegisterObject(this);
        }

        /// <summary>
        /// Reference to <see cref="StateManager"/>
        /// </summary>
        protected StateManager stateManager { get; set; }

        /// <summary>
        /// Gets or sets state priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets <see cref="DesignController"/> for easier access.
        /// </summary>
        protected DesignController designController { get { return stateManager.DesignController; } }

        /// <summary>
        /// Gets <see cref="StateController"/> for easier access.
        /// </summary>
        protected StateController controller { get { return stateManager.Controller; } }

        /// <summary>
        /// Gets <see cref="Transform"/> for easier access.
        /// </summary>
        protected Transform transform { get { return stateManager.transform; } }

        /// <summary>
        /// Starts on beginning of the state.
        /// </summary>
        public virtual void OnEnter_State()
        {
            if (designController != null)
            {
                designController.StartTask(this.GetType().Name);
            }
        }

        /// <summary>
        /// Checks when is the best time for state to become active.
        /// </summary>
        public virtual void Update_State()
        {
        }

        /// <summary>
        /// While state is active.
        /// </summary>
        public virtual void WhileActiveFixed_State()
        {
        }

        /// <summary>
        /// While state is active.
        /// </summary>
        public virtual void WhileActive_State()
        {
        }

        /// <summary>
        /// Happens when state is being swapped by other state.
        /// </summary>
        public virtual void OnExit_State()
        {
            if (designController != null)
            {
                designController.StopTask(this.GetType().Name);
            }
        }
    }
}
