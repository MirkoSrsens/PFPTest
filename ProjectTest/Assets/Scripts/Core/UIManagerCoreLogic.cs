using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.InjectionData;
using Assets.Scripts.UI;
using DiContainerLibrary.DiContainer;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public partial class UIManager : SingletonBehaviour<UIManager>
    {
        #region Routines
        /// <summary>
        /// Plays intro sequence iterating over provided <see cref="IntroSequence"/> scriptable object.
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region Show/Hide UI elements
        /// Maybe in the future other logic will go in <see cref="ShowLoginScreen"/> and <see cref="CloseLoginScreen"/>.
        public void ShowLoginScreen()
        {
            CloseAllExcept(null, new GameObject[] { _signInPanel, _loginPanel });
        }

        public void ShowMainMenu()
        {
            PlayfabManager.Inst.GetCurrencyData();
            CloseAllExcept(_mainMenuPanel);
        }

        public void ShowShop()
        {
            PlayfabManager.Inst.GetCatalogItems();
            CloseAllExcept(_shopPanel);
        }

        public void ShowLoseScreen()
        {
            var gameInformation = DiContainerInitializor.Register<IGameInformation>();

            if(gameInformation != null)
            {
                _loseScoreText.text = gameInformation.Score.ToString();
                _loseHighscoreText.text = gameInformation.CurrentHighScore.ToString();
            }

            CloseAllExcept(_losePanel);
        }

        public void ShowInventory()
        {
            PlayfabManager.Inst.GetUserInventory();
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

        public void ShowTimeCurrencyPanel()
        {
            CloseAllExcept(_energyRechargePanel);
        }

        /// <summary>
        /// This value will be used if multiple request are being loaded.
        /// Basicly it will prevent closing of loading screen until all requests are done.
        /// </summary>
        private int depth = 0;
        public void ShowLoadingPanel()
        {
            depth++;
            _loadingPanel.SetActive(true);
        }

        public void HideLoadingPanel()
        {
            if (depth > 0)
            {
                depth--;
            }

            if (depth == 0)
            {
                _loadingPanel.SetActive(false);
            }
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
            //_infoPanel.gameObject.SetActive(false);
            //_loadingPanel.gameObject.SetActive(false);
            _energyRechargePanel.gameObject.SetActive(false);

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
        #endregion

        #region OnClick_Events
        public void OnClick_PlayfabLogin()
        {
            PlayfabManager.Inst.PerformLogin(_username.text, _password.text,
                success =>
                {
                    GameManager.Inst.StartMainMenuState();
                }, null);
        }

        public void OnClick_StartGame()
        {
            PlayfabManager.Inst.StartGameRequest(
                (id) =>
                {
                    CloseAllExcept(null);
                    GameManager.Inst.OnStartGame(id);
                });
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
        #endregion

        #region Callbacks from other scripts 
        public void DisplayChoseValueOption(CatalogItem catalogItem)
        {
            CloseAllExcept(null, new GameObject[] { _choseCurrencyPanel.gameObject, _shopPanel });
            _choseCurrencyPanel.SpawnCurrencyOptions(catalogItem);
        }

        public void OpenItemDetails(string itemId)
        {
            _inventoryItemDetails.ActivateItemDetails(itemId);
            PlayfabManager.Inst.GetCatalogItems();
        }

        /// <summary>
        /// Swaps states to main menu. Used when we want to go back to main menu from playing state.
        /// </summary>
        public void GoToMainMenu()
        {
            GameManager.Inst.StartMainMenuState();
        }

        public void UpdateInGameScore(int number)
        {
            _inGameHighscore.text = number.ToString();
        }

        /// <summary>
        /// Updates value of timer on time currency 'Energy'
        /// </summary>
        /// <param name="rechargeTimeMax">The amount to set recharge to.</param>
        /// <param name="energyRechargeLimit">The maximum amount of rechargeable energy.</param>
        public void UpdateRechargeTimeOfEnergyValue(int rechargeTime, int energyRechargeLimit)
        {
            _energyRechargeTimeText.text = string.Format(Const.FORMAT_ENERGY_CURRENCY_TEXT, energyRechargeLimit.ToString(), rechargeTime.ToString());
        }
        #endregion
    }
}
