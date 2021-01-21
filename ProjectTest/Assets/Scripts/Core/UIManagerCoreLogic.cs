using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.InjectionData;
using Assets.Scripts.UI;
using DiContainerLibrary.DiContainer;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public partial class UIManager : SingletonBehaviour<UIManager>
    {
        public IEnumerator PlayIntroSequence()
        {
            CloseAllExcept(_introPanel);

            foreach (var item in _introSequenceData.SequenceOfObjects)
            {
                var image = Instantiate(item.SplashImage, _introPanel.transform).GetComponent<Image>();
                image.ChangeAlpha(0);
                while (image.color.a < 0.99f)
                {
                    image.ChangeAlpha(image.color.a + _alphaChangeSpeed * Time.deltaTime);
                    // yeh you can predefine this but its intro sequence.
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(item.Duration);

                Destroy(image.gameObject);
            }

            _introPanel.SetActive(false);
        }

        /// Maybe in the future other logic will go in <see cref="ShowLoginScreen"/> and <see cref="CloseLoginScreen"/>.
        public void ShowLoginScreen()
        {
            CloseAllExcept(null, new GameObject[] { _signInPanel, _loginPanel });
        }

        public void ShowMainMenu()
        {
            CloseAllExcept(_mainMenuPanel);
        }

        public void ShowShop(bool refreshCatalog = true)
        {
            if (refreshCatalog)
            {
                PlayfabManager.Inst.GetCatalogItems();
            }
            CloseAllExcept(_shopPanel);
        }

        public void ShowLoseScreen()
        {
            // Don't over complicate just copy one value to another.
            _loseHighscoreText.text = _inGameHighscore.text;
            CloseAllExcept(_losePanel);
        }

        public void ShowInventory(bool refreshInventory = true)
        {
            if (refreshInventory)
            {
                PlayfabManager.Inst.GetUserInventory();
            }
            CloseAllExcept(_inventoryPanel);
        }

        public void ShowUpgrade(bool refreshData)
        {
            if (refreshData)
            {
                // Get data of stats.
                PlayfabManager.Inst.GetUserReadonlyData();
            }
            CloseAllExcept(_upgradePanel.gameObject);
        }

        public void ShowInGamePannel()
        {
            // just reset UI number if any exists.
            _inGameHighscore.text = "0";

            CloseAllExcept(_inGamePanel);
        }

        public void ShowLeaderboard(bool refreshData = true)
        {
            if (refreshData)
            {
                PlayfabManager.Inst.GetLeaderboardData();
            }

            CloseAllExcept(_leaderboardPanel);
        }

        public void ShowRegister()
        {
            CloseAllExcept(null, new GameObject[] { _signInPanel, _registerPanel });
        }

        public void CloseAllExcept(GameObject panel = null, GameObject[] activate = null)
        {
            //Careful this will trigger on enable/disable for activating element.
            _signInPanel.gameObject.SetActive(false);
            _registerPanel.gameObject.SetActive(false);
            _introPanel.SetActive(false);
            _loginPanel.SetActive(false);
            _mainMenuPanel.SetActive(false);
            _shopPanel.SetActive(false);
            _choseCurrencyPanel.gameObject.SetActive(false);
            _inventoryPanel.SetActive(false);
            _inventoryItemDetails.gameObject.SetActive(false);
            _upgradePanel.gameObject.SetActive(false);
            _losePanel.gameObject.SetActive(false);
            _inGamePanel.gameObject.SetActive(false);
            _leaderboardPanel.gameObject.SetActive(false);
            _infoPanel.gameObject.SetActive(false);

            if (panel != null)
            {
                panel.SetActive(true);
            }

            if (activate != null)
            {
                foreach (var item in activate)
                {
                    item.SetActive(true);
                }
            }
        }

        public void OnPlayfabLoginClicked()
        {
            PlayfabManager.Inst.PerformLogin(_username.text, _password.text,
                success =>
                {
                    GameManager.Inst.StartMainMenuState();
                }, null);
        }

        public void DisplayChoseValueOption(CatalogItem catalogItem)
        {
            // We dont want to close shop panel in background just for aesthetics reasons :D.
            _choseCurrencyPanel.gameObject.SetActive(true);
            _choseCurrencyPanel.SpawnCurrencyOptions(catalogItem);
        }

        public void OpenItemDetails(string itemId)
        {
            _inventoryItemDetails.ActivateItemDetails(itemId);
            PlayfabManager.Inst.GetCatalogItems();
        }


        public void OnClick_StartGame()
        {
            PlayfabManager.Inst.StartGameRequest(
                () =>
                {
                    CloseAllExcept(null);
                    GameManager.Inst.OnStartGame();
                });
        }

        public void GoToMainMenu()
        {
            GameManager.Inst.StartMainMenuState();
        }

        public void UpdateInGameScore(int number)
        {
            _inGameHighscore.text = number.ToString();
        }

        public void SubmitHighScore()
        {
            // Perform decrypt and then encrypt in AES and send that to server which
            // contains decrypt algorithm. RSA unfortunately requires some libraries 
            // on cloud which are not available.
            var gameInformation = DiContainerInitializor.Register<IGameInformation>();

            if (gameInformation != null)
            {
                PlayfabManager.Inst.SubmitHighscore(Security.MagicHat(gameInformation.Score.ToString()));
            }
        }

        public void OnClick_RegisterUser()
        {
            var data = new RegisterData(_registerUsername.text, _registerEmail.text, _registerPassword.text, _registerPasswordConfirm.text);

            Security.ValidateData(data,
                () =>
                {
                    PlayfabManager.Inst.RegisterUser(data);
                },
                failed =>
                {
                    _infoPanel.Setup(failed);
                });
        }

        public void OnClick_SignOut()
        {
            PlayfabManager.Inst.SignOut();
        }
    }
}
