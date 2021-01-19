using Assets.Scripts.Data.Events;
using Assets.Scripts.Data.Scriptable;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [Header("Intro data")]
        [SerializeField]
        private IntroSequence _introSequenceData;

        [SerializeField]
        private GameObject _introPanel;

        [SerializeField]
        private float _alphaChangeSpeed;

        [Header("Playfab Data")]

        [SerializeField]
        private GameObject _loginPanel;

        [SerializeField]
        private GameObject _genericErrorBox;

        [Header("Login elements")]
        [SerializeField]
        private InputField _username;

        [SerializeField]
        private InputField _password;

        [Header("MainMenu elements")]
        [SerializeField]
        private GameObject _mainMenuPanel;

        [SerializeField]
        private Text _usernameDisplay;

        [SerializeField]
        // Soft currency
        private Text _goldCount;

        [SerializeField]
        private Text _energyCount;

        [SerializeField]
        // Hard currency
        private Text _diamondCount;


        private void OnEnable()
        {
            PlayfabManager.Inst.RefreshCurrencyDataEvent += OnCurrencyDataRefresh;
            PlayfabManager.Inst.RefreshUserDetailsData += OnUserInfoAcquired;
        }

        private void OnDisable()
        {
            PlayfabManager.Inst.RefreshCurrencyDataEvent -= OnCurrencyDataRefresh;
            PlayfabManager.Inst.RefreshUserDetailsData -= OnUserInfoAcquired;
        }

        public IEnumerator PlayIntroSequence()
        {
            CloseAllExcept(_introPanel);

            foreach (var item in _introSequenceData.SequenceOfObjects)
            {
                var image = Instantiate(item.SplashImage, _introPanel.transform).GetComponent<Image>();
                image.ChangeAlpha(0);
                while(image.color.a < 0.99f)
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
            CloseAllExcept(_loginPanel);
        }

        public void ShowMainMenu()
        {
            CloseAllExcept(_mainMenuPanel);
        }

        private void CloseAllExcept(GameObject panel = null)
        {
            _introPanel.SetActive(false);
            _loginPanel.SetActive(false);
            _mainMenuPanel.SetActive(false);

            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        public void OnPlayfabLoginClicked()
        {
            PlayfabManager.Inst.PerformLogin(_username.text, _password.text, 
                success =>
                {
                    GameManager.Inst.StartMainMenuState();
                },
                failed =>
                {
                    DisplayGenericPlayfabError(PlayfabManager.Inst.GenericMessage(failed));
                });
        }

        private void OnCurrencyDataRefresh(object sender, PlayfabRefreshCurrencyEventArgs eventArgs)
        {
            _goldCount.text = eventArgs.Gold.ToString();
            _energyCount.text = eventArgs.Energy.ToString();
        }

        private void OnUserInfoAcquired(object sender, PlayfabUserInfoEventArgs eventArgs)
        {

        }

        public void DisplayGenericPlayfabError(string message)
        {
        }

    }
}
