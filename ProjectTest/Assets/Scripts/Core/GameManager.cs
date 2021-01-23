using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates
{
    Intro, // Intro splash screen sequence
    Login, // Waiting for user to login
    MainMenu, // Main menu
    Playing, // Playing the game
    Lose, // On lose game.
}

namespace Assets.Scripts.Core
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        /// <summary>
        /// This field holds reference to main menu camera which will be turned on/off between <see cref="GameStates.MainMenu"/> and <see cref="GameStates.Playing"/>
        /// </summary>
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
        /// Encrypt for session Id.
        /// </summary>
        private string SessionId { get { return Security.Decrypt(_sessionId); } set { _sessionId = Security.Encrypt(value.ToString()); } }


        private void Start()
        {
            CurrentStateOfGame = GameStates.Intro;
            // This will make sure nobody changed stats in lobby and will trigger application of the stats to game. 
            SceneManager.sceneLoaded += new UnityEngine.Events.UnityAction<Scene, LoadSceneMode>((a,b) => PlayfabManager.Inst.GetUserReadonlyData());
            
            // Dont want to use this in editor, takes time.
#if !UNITY_EDITOR
           //TrustContractManager.Inst.Sign(UIManager.Inst.PlayIntroSequence(), StartLoginState);
#else
            StartLoginState();
#endif
        }

        /// <summary>
        /// Handle when enerting login state.
        /// </summary>
        public void StartLoginState()
        {
            CurrentStateOfGame = GameStates.Login;

            // If there is cached data on phone preform cold login.
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

        /// <summary>
        /// Starts main menu state after login was successfully performed.
        /// </summary>
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

        /// <summary>
        /// On game started.
        /// </summary>
        /// <param name="id"><see cref="SessionId"/></param>
        public void OnStartGame(string id)
        {
            CurrentStateOfGame = GameStates.Playing;
            mainMenuCamera.SetActive(false);
            SessionId = id;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if ((scene.name == Const.FLAPPY_BIRD_SCENE && scene.isLoaded))
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
            SceneManager.LoadScene(Const.FLAPPY_BIRD_SCENE, LoadSceneMode.Additive);

            UIManager.Inst.ShowInGamePannel();
        }


        /// <summary>
        /// On game lost.
        /// </summary>
        public void OnGameLose()
        {
            CurrentStateOfGame = GameStates.Lose;
            UIManager.Inst.ShowLoseScreen();

            // Perform decrypt and then encode and send that to server which
            // contains decrypt algorithm. RSA unfortunately requires some libraries 
            // on cloud which are not available.
            var gameInformation = DiContainerInitializor.Register<IGameInformation>();
            if (gameInformation != null && gameInformation.Score > gameInformation.CurrentHighScore)
            {
                PlayfabManager.Inst.SubmitHighscore(Security.MagicHat(gameInformation.Score.ToString()));
            }

            // Don't waste time if there is nothing to add.
            if (gameInformation.CoinCollected > 0)
            {
                PlayfabManager.Inst.AddCoins(SessionId, gameInformation.CoinCollected);
            }
        }
    }
}
