public sealed class Const
{
    // Constants values
    public const string PLAYFAB_USERNAME = "PLAYFAB_USERNAME";
    public const string PLAYFAB_PASSWORD = "PLAYFAB_PASSWORD";
    public const string GOLD_CURRENCY = "GL";
    public const string ENERGY_CURRENCY = "EG";
    public const string DIAMONDS_CURRENCY = "DM";
    public const string STATISTIC_NAME = "Highscore";
    public const int HIGHSCORE_START_INDEX = 0;
    public const int HIGHSCORE_RANGE_COUNT = 10;
    public const string DEFAULT_LEADERBOARD_NAME = "-";
    public const int DEFAULT_LEADERBOARD_SCORE = 0;
    public const string FLAPPY_BIRD_SCENE = "FlappyBird";
    public const string NETWORK_ROOM_SCENE = "NetworkRoom";
    public const string DISPLAY_CURRENCY_FORMAT = "{0}: {1} /";
    public const char NEW_LINE = '\n';
    public const string ON_REGISTRATION_SUCCESS = "New user registered, please login!";
    public const int VALIDATION_PASSWORD_LENGHT = 6;
    public const int VALIDATION_USERNAME_LENGHT = 5;
    public const string VALIDATION_PASSWORD_MISMATCH = "Passwords needs to match";
    public static readonly string VALIDATION_PASSWORD_SHORT = string.Format( "Password needs to be at least {0} letters long", VALIDATION_PASSWORD_LENGHT);
    public const string VALIDATION_INVALID_EMAIL = "Not valid email!";
    public static readonly string VALIDATION_USERNAME_SHORT = string.Format("Username must have more than {0} letters", VALIDATION_USERNAME_LENGHT);
    public const string STAT_MOVEMENT_SPEED = "MovementSpeed";
    public const string STAT_WING_FLAPS_STRENGHT = "WingFlapsStrenght";
    public const string SUFIX_CLONE = "(Clone)";
    public const string FORMAT_ENERGY_CURRENCY_TEXT = "Time until next currency recharges will not increment if your energy is higher than {0}: \n {1}";
    public const string FORMAT_STATS_TEXT = "LEVEL: {0} \n COST: {1}";

    // TAGS
    public const string TAG_POINT = "Point";
    public const string TAG_KILLER = "Killer";
    public const string TAG_COIN = "Coin";
    public const string TAG_PLAYER = "Player";

    // VALUE MODIFIERS
    public const int POINT_WORTH = 1;
    public const int COIN_WORTH = 1;



    // Constants functions
    public const string FUNC_SUBMIT_HIGHSCORE = "SubmitHighscore";
    public const string FUNC_ADD_COINS = "AddCoins";
    public const string FUNC_UPGRADE_STAT = "UpgradeStat";
    public const string FUNC_USE_ITEM = "UseItem";
    public const string FUNC_BUY_ITEM = "BuyItem";
    public const string FUNC_CAN_START_GAME = "CanStartGame";
}
