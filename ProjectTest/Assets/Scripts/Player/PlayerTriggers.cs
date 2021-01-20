using General.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Example how to externally manipulate object states. 
    /// </summary>
    public class PlayerTriggers : MonoBehaviour
    {
        private const string Point = "Point";
        private const string Killer = "Killer";

        private Death deathState { get; set; }
        private StateController stateController { get; set; }

        private void Awake()
        {
            deathState = GetComponent<Death>();
            stateController = GetComponent<StateController>();
        }


        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            if (other.gameObject.tag == Point)
            {

            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Killer)
            {
                stateController.SwapState(deathState);
            }
        }
    }
}
