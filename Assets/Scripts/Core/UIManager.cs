using Assets.Scripts.Data.Scriptable;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public partial class UIManager : SingletonBehaviour<UIManager>
    {
        [Header("Intro data")]
        [SerializeField]
        private IntroSequence _introSequenceData;

        [SerializeField]
        private GameObject _introPanel;

        [SerializeField]
        private float _alphaChangeSpeed;

        [Header("Login elements")]
        [SerializeField]
        private GameObject _signInPanel;

        [SerializeField]
        private GameObject _loginPanel;

        [SerializeField]
        private InputField _username;

        [SerializeField]
        private InputField _password;

        [Header("Register elements")]
        [SerializeField]
        private GameObject _registerPanel;

        [SerializeField]
        private InputField _registerUsername;

        [SerializeField]
        private InputField _registerEmail;

        [SerializeField]
        private InputField _registerPassword;

        [SerializeField]
        private InputField _registerPasswordConfirm;

        [Header("MainMenu elements")]
        [SerializeField]
        private GameObject _mainMenuPanel;

        [SerializeField]
        private Text _usernameDisplay;

        [SerializeField]
        // Soft currency.
        private Text _goldCount;

        [SerializeField]
        // Time currency.
        private Text _energyCount;

        [SerializeField]
        // Hard currency.
        private Text _diamondCount;

        [SerializeField]
        private GameObject _energyRechargePanel;

        [SerializeField]
        private Text _energyRechargeTimeText;

        [Header("Shop elements")]
        [SerializeField]
        private GameObject _shopPanel;

        [SerializeField]
        private GameObject _catalogItemPanel;

        [SerializeField]
        private CatalogItem _catalogItemPrefab;

        [SerializeField]
        private ChoseCurrencyPanel _choseCurrencyPanel;


        [Header("InventoryItems elements")]
        [SerializeField]
        private GameObject _inventoryPanel;

        [SerializeField]
        private GameObject _inventoryItemListPanel;

        [SerializeField]
        private InventoryItem _inventoryItemPrefab;

        [SerializeField]
        private InventoryItemDetails _inventoryItemDetails;


        [Header("Upgrade panel elements")]
        [SerializeField]
        private UpgradePanel _upgradePanel;


        [Header("Info panel elements")]
        [SerializeField]
        private InfoPanel _infoPanel;

        [Header("Lose panel elements")]
        [SerializeField]
        private GameObject _losePanel;

        [SerializeField]
        private Text _loseScoreText;

        [SerializeField]
        private Text _loseHighscoreText;

        [Header("In game UI elements")]
        [SerializeField]
        private GameObject _inGamePanel;

        [SerializeField]
        private Text _inGameHighscore;

        [Header("Leaderboard UI elements")]
        [SerializeField]
        private GameObject _leaderboardPanel;

        [SerializeField]
        private GameObject _leaderboardListPanel;

        [SerializeField]
        private LeaderboardEntity _leaderBoardEntityPrefab;

        [Header("Loading panel")]
        [SerializeField]
        private GameObject _loadingPanel;

        [Header("Coop elements")]
        [SerializeField]
        private GameObject _coopPanel;

        [SerializeField]
        private InputField _coopUsernameText;
    }
}
