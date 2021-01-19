using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    Intro,
    Login,
    MainMenu,
    Playing,
}

namespace Assets.Scripts.Core
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        /// <summary>
        /// Gets or sets current state of the game. Allowing us to inspect closer what state did we stuck with
        /// and potentially recover from game braking issues.
        /// </summary>
        public GameStates CurrentStateOfGame { get; set; }

        private void Start()
        {
            CurrentStateOfGame = GameStates.Intro;

            // Dont want to use this in editor, takes time.
#if !UNITY_EDITOR
            TrustContractManager.Inst.Sign(UIManager.Inst.PlayIntroSequence(), StartLoginState);
#else
            StartLoginState();
#endif
        }

        public void StartLoginState()
        {
            CurrentStateOfGame = GameStates.Login;

            if(PlayfabManager.Inst.CheckIfLoginIsCached())
            {
                PlayfabManager.Inst.ColdLogin(
                    success =>
                    {
                        StartMainMenuState();
                    },
                    error =>
                    {
                        Debug.LogError("Something went wrong during automatic login");
                    });
            }

            UIManager.Inst.ShowLoginScreen();
        }

        public void StartMainMenuState()
        {
            CurrentStateOfGame = GameStates.MainMenu;
            PlayfabManager.Inst.GetCurrencyData();
            UIManager.Inst.ShowMainMenu();
        }
    }
}
