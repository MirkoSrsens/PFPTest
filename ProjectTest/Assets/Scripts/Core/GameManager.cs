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
    Lose,
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
            mainMenuCamera.SetActive(true);
            PlayfabManager.Inst.GetUserData();
            PlayfabManager.Inst.GetCurrencyData();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if ((scene.name == "FlappyBird" && scene.isLoaded))
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }

            UIManager.Inst.ShowMainMenu();
        }

        public void OnStartGame()
        {
            CurrentStateOfGame = GameStates.Playing;
            mainMenuCamera.SetActive(false);

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if ((scene.name == "FlappyBird" && scene.isLoaded))
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
            SceneManager.LoadScene("FlappyBird", LoadSceneMode.Additive);
            UIManager.Inst.ShowInGamePannel();
        }

        public void OnGameLose()
        {
            CurrentStateOfGame = GameStates.Lose;
            UIManager.Inst.ShowLoseScreen();
        }
    }
}
