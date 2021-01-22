using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
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

        /// <summary>
        /// Holds key to session allowing us to check if anyone messed with game.
        /// </summary>
        private string _sessionId;

        /// <summary>
        /// Encryptor for session Id.
        /// </summary>
        private string SessionId { get { return Security.Decrypt(_sessionId); } set { _sessionId = Security.Encrypt(value.ToString()); } }


        private void Start()
        {
            CurrentStateOfGame = GameStates.Intro;
            // This will make sure nobody changed stats in lobby and will trigger application of the stats to game. 
            SceneManager.sceneLoaded += new UnityEngine.Events.UnityAction<Scene, LoadSceneMode>((a,b) => PlayfabManager.Inst.GetUserReadonlyData());
            
            // Dont want to use this in editor, takes time.
#if !UNITY_EDITOR && ASDESDEAFAFA
           //TrustContractManager.Inst.Sign(UIManager.Inst.PlayIntroSequence(), StartLoginState);
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
            mainMenuCamera.SetActive(true);
            PlayfabManager.Inst.GetUserAccountInformationData();

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

        public void OnStartGame(string id)
        {
            CurrentStateOfGame = GameStates.Playing;
            mainMenuCamera.SetActive(false);
            SessionId = id;

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

            // Perform decrypt and then encrypt in AES and send that to server which
            // contains decrypt algorithm. RSA unfortunately requires some libraries 
            // on cloud which are not available.
            var gameInformation = DiContainerInitializor.Register<IGameInformation>();
            if (gameInformation != null && gameInformation.Score > gameInformation.CurrentHighScore)
            {
                PlayfabManager.Inst.SubmitHighscore(Security.MagicHat(gameInformation.Score.ToString()));
            }

            // Dont waste time if there is nothing to add.
            if (gameInformation.CoinCollected > 0)
            {
                PlayfabManager.Inst.AddCoins(SessionId, gameInformation.CoinCollected);
            }
        }
    }
}
