using System;
using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Core state for all actions.
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets state priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets <see cref="DesignController"/> for easier access.
        /// </summary>
        protected DesignController designController { get; set; }

        /// <summary>
        /// Gets <see cref="StateController"/> for easier access.
        /// </summary>
        public StateController controller { get; private set; }

        private void Awake()
        {
            designController = GetComponent<DesignController>();
            controller = GetComponent<StateController>();
        }

        /// <summary>
        /// Used for initializing one time components and values.
        /// </summary>
        protected virtual void Initialization_State()
        {
            DiContainerLibrary.DiContainer.DiContainerInitializor.RegisterObject(this);
        }

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

        private void Start()
        {
            Initialization_State();
        }

        public void Update()
        {
            Update_State();
        }
    }
}
