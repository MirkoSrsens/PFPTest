using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField]
        private GameObject mainMenuCamera;

        /// <summary>
        /// Gets or sets current state of the game. Allowing us to inspect closer what state did we stuck with
        /// and potentially recover from game braking issues.
        /// </summary>
        public GameStates CurrentStateOfGame { get; set; }

        private void Start()
        {
            // Just so im sure physics wont be wierd when i go from 300 fps on pc to something on mobile.
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            CurrentStateOfGame = GameStates.Intro;

            // Dont want to use this in editor, takes time.
////#if !UNITY_EDITOR
////            //TrustContractManager.Inst.Sign(UIManager.Inst.PlayIntroSequence(), StartLoginState);
////#else
            StartLoginState();
//#endif
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
            PlayfabManager.Inst.GetUserData();
            PlayfabManager.Inst.GetCurrencyData();
            UIManager.Inst.ShowMainMenu();
        }

        public void StartGame()
        {
            CurrentStateOfGame = GameStates.Playing;
            mainMenuCamera.SetActive(false);
            SceneManager.LoadScene("FlappyBird", LoadSceneMode.Additive);
        }
    }
}
