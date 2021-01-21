﻿using Assets.Scripts.Core;
using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
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
        private IGameInformation gameInformation { get; set; }

        private const string Point = "Point";
        private const string Killer = "Killer";

        private Death deathState { get; set; }
        private StateController stateController { get; set; }

        private void Start()
        {
            // Example of non attribute injection.
            gameInformation = DiContainerInitializor.Register<IGameInformation>();
            deathState = GetComponent<Death>();
            stateController = GetComponent<StateController>();
        }

        private void OnTriggerEnter2D(UnityEngine.Collider2D other)
        {
            if (other.gameObject.tag == Point)
            {
                gameInformation.Score += 1;
                UIManager.Inst.UpdateInGameScore(gameInformation.Score);
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
