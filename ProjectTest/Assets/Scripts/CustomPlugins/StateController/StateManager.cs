using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.State
{
    public abstract class StateManager : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets state controller.
        /// </summary>
        public StateController Controller { get; protected set; }


        /// <summary>
        /// Gets or sets design controller
        /// </summary>
        public DesignController DesignController { get; set; }

        /// <summary>
        /// All States on object.
        /// </summary>
        public List<State> AllStates { get; set; }

        protected virtual void Start()
        {
            DiContainerLibrary.DiContainer.DiContainerInitializor.RegisterObject(this);
            Controller = GetComponent<StateController>();
            DesignController = GetComponent<DesignController>();
            AllStates = new List<State>();
            AllStates = AddStates();
        }

        public abstract List<State> AddStates(); 

        protected virtual void Update()
        {
            if(AllStates != null)
            {
                for(int i=0; i<AllStates.Count;i++)
                {
                    AllStates[i].Update_State();
                }
            }
        }

        internal void CallCorutine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }

        internal void Spawn(GameObject obj, Vector2 position, Quaternion rotation, Transform parent)
        {
            Instantiate(obj, position, rotation, parent);
        }
    }
}
