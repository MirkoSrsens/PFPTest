using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.Events;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Data.Events.PlayfabCatalogItemsEventArgs;
using static Assets.Scripts.Data.Events.PlayfabErrorHandlingEventArgs;
using static Assets.Scripts.Data.Events.PlayfabOnApiCallEndEventArgs;
using static Assets.Scripts.Data.Events.PlayfabOnApiCallStartEventArgs;
using static Assets.Scripts.Data.Events.PlayfabOnUserRegisteredEventArgs;
using static Assets.Scripts.Data.Events.PlayfabRefreshCurrencyEventArgs;
using static Assets.Scripts.Data.Events.PlayfabRefreshLeaderboardsDataEventArgs;
using static Assets.Scripts.Data.Events.PlayfabUserInfoEventArgs;
using static Assets.Scripts.Data.Events.PlayfabUserInventoryEventArgs;
using static Assets.Scripts.Data.Events.PlayfabUserReadonlyDataEventArgs;

namespace Assets.Scripts.Core
{
    public class PlayfabManager : SingletonBehaviour<PlayfabManager>
    {


        /// <summary>
        /// Every time we request to change currency fire sequence of elements that are affected by that.
        /// </summary>
        public event PlayfabRefreshCurrencyEventHandler OnRefreshCurrencyDataEvent;

        /// <summary>
        /// Can be hard coded in methods but if we have change user name feature this might come in handy.
        /// </summary>
        public event PlayfabUserInfoEventHandler OnRefreshUserDetailsData;

        /// <summary>
        /// When we get catalog items do something with them on UI part. Left space for some platform specific subscriptions
        /// special deals etc.
        /// </summary>
        public event PlayfabCatalogItemsEventHandler OnRefreshCatalogItems;

        /// <summary>
        /// When we get items we should display then on user UI so he can do whatever with them.
        /// </summary>
        public event PlayfabUserInventoryEventHandler OnRefreshPlayerInventory;

        /// <summary>
        /// Used when we get response on get user readonly data.
        /// </summary>
        public event PlayfabUserReadonlyDataEventHandler OnRefreshUserReadonlyData;

        /// <summary>
        /// Used for error handling.
        /// </summary>
        public event PlayfabErrorHandlingEventHandler OnErrorEvent;

        /// <summary>
        /// Used for start of api calls .
        /// </summary>
        public event PlayfabOnApiCallStartEventHandler OnApiCallStart;

        /// <summary>
        /// Used for end of api calls .
        /// </summary>
        public event PlayfabOnApiCallEndEventHandler OnApiCallEnd;

        /// <summary>
        /// Used for error handling.
        /// </summary>
        public event PlayfabRefreshLeaderboardsDataEventHandler OnLeaderboardRefresh;

        /// <summary>
        /// Triggers when new user is registered.
        /// </summary>
        public event PlayfabOnUserRegisteredEventHandler OnUserRegistered;

        /// <summary>
        /// Used for getting current user data from <see cref="GetUserAccountInformationData"/>.
        /// </summary>
        public string PlayfabCurrentUserID { get; private set; }

        /// <summary>
        /// Check if there is some previous login that is saved. Perform <see cref="ColdLogin(Action{LoginResult}, Action{PlayFabError})"/> if true.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfLoginIsCached()
        {
            return PlayerPrefs.HasKey(Const.PLAYFAB_USERNAME) && PlayerPrefs.HasKey(Const.PLAYFAB_PASSWORD);
        }

        /// <summary>
        /// Cold login means there is some encrypted data in device memmory.
        /// </summary>
        /// <param name="success">On success execute this action.</param>
        /// <param name="failed">On failed perform this action.</param>
        internal void ColdLogin(Action<LoginResult> success, Action<PlayFabError> failed)
        {
            var username = Security.Decrypt(PlayerPrefs.GetString(Const.PLAYFAB_USERNAME));
            var password = Security.Decrypt(PlayerPrefs.GetString(Const.PLAYFAB_PASSWORD));

            PerformLogin(username, password, success, failed);
        }

        /// <summary>
        /// Perform user login if it fails attempt to do email login (since user might enter email).
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="success">On success perform this action.</param>
        /// <param name="failed">On fail perform this action.</param>
        public void PerformLogin(string username, string password, Action<LoginResult> success = null, Action<PlayFabError> onFailed = null)
        {
            var loginRequest = new LoginWithPlayFabRequest()
            {
                Username = username,
                Password = password,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
                {
                    GetPlayerProfile = true,
                    ProfileConstraints = new PlayerProfileViewConstraints()
                    {
                        ShowDisplayName = true,
                        ShowContactEmailAddresses = true
                    }
                },
                TitleId = PlayFabSettings.TitleId
            };

            OnApiCallStart();
            PlayFabClientAPI.LoginWithPlayFab(loginRequest,
                succ =>
                {
                    OnApiCallEnd();
                    Debug.Log("Login with username was successful");
                    PlayfabCurrentUserID = succ.PlayFabId;

                    PlayerPrefs.SetString(Const.PLAYFAB_USERNAME, Security.Encrypt(username));
                    PlayerPrefs.SetString(Const.PLAYFAB_PASSWORD, Security.Encrypt(password));

                    if (success != null)
                    {
                        success(succ);
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    if (failed.Error == PlayFabErrorCode.AccountNotFound || failed.Error == PlayFabErrorCode.InvalidParams)
                    {
                        // Assume user name might be email actually. Don't trigger fail since it will be handled
                        // by login with email method.
                        PerformLoginWithEmail(username, password, success, onFailed);
                        return;
                    }

                    Debug.LogError(failed.ToString());
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    if (onFailed != null)
                    {
                        onFailed(failed);
                    }
                });
        }

        /// <summary>
        /// Perform login with email. In this case only fires after username login fails.
        /// </summary>
        /// <param name="email">The username of account.</param>
        /// <param name="password">The password of account.</param>
        /// <param name="onSuccess">On success perform this.</param>
        /// <param name="failed">On failed perform this.</param>
        private void PerformLoginWithEmail(string email, string password, Action<LoginResult> onSuccess, Action<PlayFabError> onFailed)
        {
            var loginRequest = Mappings.CreateEmailRequest(email, password);

            OnApiCallStart();
            PlayFabClientAPI.LoginWithEmailAddress(loginRequest,
                success =>
                {
                    OnApiCallEnd();
                    Debug.Log("Login with email was successful");
                    PlayfabCurrentUserID = success.PlayFabId;

                    PlayerPrefs.SetString(Const.PLAYFAB_USERNAME, Security.Encrypt(email));
                    PlayerPrefs.SetString(Const.PLAYFAB_PASSWORD, Security.Encrypt(password));

                    if (onSuccess != null)
                    {
                        onSuccess(success);
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    Debug.LogError(failed.ToString());
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));

                    if (failed != null)
                    {
                        onFailed(failed);
                    }
                });
        }

        /// <summary>
        /// Get user data. We use this method to get stats and perform security checks with cloud scripts.
        /// </summary>
        public void GetUserAccountInformationData()
        {
            var request = Mappings.CreateAccountInfoRequest(PlayfabCurrentUserID);

            OnApiCallStart();
            PlayFabClientAPI.GetAccountInfo(request,
                success =>
                {
                    OnApiCallEnd();
                    var userInfo = new PlayfabUserInfoEventArgs(success.AccountInfo.Username, success.AccountInfo.PrivateInfo.Email);

                    OnRefreshUserDetailsData(this, userInfo);
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Gets all currency data for user.
        /// </summary>
        public void GetCurrencyData()
        {
            var request = new GetUserInventoryRequest(); ;

            OnApiCallStart();
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            success =>
            {
                OnApiCallEnd();
                var eventData = new PlayfabRefreshCurrencyEventArgs(success.VirtualCurrency[Const.ENERGY_CURRENCY],
                    success.VirtualCurrency[Const.GOLD_CURRENCY],
                    success.VirtualCurrency[Const.DIAMONDS_CURRENCY],
                    success.VirtualCurrencyRechargeTimes[Const.ENERGY_CURRENCY].SecondsToRecharge,
                    success.VirtualCurrencyRechargeTimes[Const.ENERGY_CURRENCY].RechargeMax);

                OnRefreshCurrencyDataEvent(this, eventData);
            },
            failed =>
            {
                OnApiCallEnd();
                OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                Debug.LogError(failed.ToString());
            });
        }

        /// <summary>
        /// Gets all catalog items. Uses default version unless specified otherwise. 
        /// </summary>
        /// <param name="catalogVersion">Catalog version</param>
        public void GetCatalogItems(string catalogVersion = null)
        {
            var request = Mappings.CreateGetCatalogItemRequest(catalogVersion);

            OnApiCallStart();
            PlayFabClientAPI.GetCatalogItems(request,
                success =>
                {
                    OnApiCallEnd();
                    OnRefreshCatalogItems(this, new PlayfabCatalogItemsEventArgs(success.Catalog));
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Gets users inventory data with optional parameters for custom tags.
        /// </summary>
        /// <param name="customTags">Custom parameters, refer to <see cref="GetUserInventoryRequest"/> for more info</param>
        public void GetUserInventory(Dictionary<string, string> customTags = null)
        {
            var request = Mappings.CreateGetUserInventoryRequest();

            OnApiCallStart();
            PlayFabClientAPI.GetUserInventory(request,
                success =>
                {
                    OnApiCallEnd();
                    var inverntoryData = new PlayfabUserInventoryEventArgs(success.Inventory);

                    OnRefreshPlayerInventory(this, inverntoryData);
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Gets users readonly data with option to specify exact keys.
        /// </summary>
        /// <param name="keys">Custom keys refer to <see cref="GetUserDataRequest"/> for more info.</param>
        public void GetUserReadonlyData(List<string> keys = null)
        {
            var request = Mappings.CreateGetUserDataRequest();

            OnApiCallStart();
            PlayFabClientAPI.GetUserReadOnlyData(request,
                success =>
                {
                    OnApiCallEnd();
                    var eventArgs = new PlayfabUserReadonlyDataEventArgs(success.Data);

                    OnRefreshUserReadonlyData(this, eventArgs);
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Upgrades specific stat. Logic is performed on server.
        /// </summary>
        /// <param name="statToUpgrade">Id of stat to update.</param>
        /// <param name="successCallback">Since its 'HOT' update we might want to do some UI refreshing when update is performed.</param>
        public void UpgradeStat(string statToUpgrade, Action successCallback)
        {
            var request = Mappings.CreateExecuteCloudScriptRequest(Const.FUNC_UPGRADE_STAT, new { Stat = statToUpgrade });

            OnApiCallStart();
            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    OnApiCallEnd();
                    if (success.Error != null)
                    {
                        OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(success.Error));
                        Debug.LogError(success.Error.ToString());
                        return;
                    }
                    if (success != null)
                    {
                        successCallback();
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Consumes item with specific id. Consume logic is performed on cloud server.
        /// </summary>
        /// <param name="itemId">Id of item to consume.</param>
        public void UseItem(string itemId)
        {
            var request = Mappings.CreateExecuteCloudScriptRequest(Const.FUNC_USE_ITEM, new { ItemId = itemId });

            OnApiCallStart();
            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    OnApiCallEnd();

                    if (success.Error == null)
                    {
                        // Just try to get it again refresh will then be performed.
                        GetUserInventory();
                    }
                    else
                    {
                        OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(success.Error));
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Buys specific item, with specific currency. (Little bit of modification of intended behaviour.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="currencySelected"></param>
        public void BuyItem(string itemId, string currencySelected)
        {
            var request = Mappings.CreateExecuteCloudScriptRequest(Const.FUNC_BUY_ITEM, new { ItemId = itemId, currencyType = currencySelected });

            OnApiCallStart();
            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    OnApiCallEnd();

                    if (success.Error == null)
                    {
                        Debug.LogFormat("Item with id {0} bought", itemId);
                    }
                    else
                    {
                        OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(success.Error));
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Checks with server if enough energy is present. Then starts the game.
        /// </summary>
        /// <param name="onSuccess">Action to execute. String parameter is session ID that will be used to check validity of game session
        /// before updating values of coins and score on server.</param>
        public void StartGameRequest(Action<string> onSuccess)
        {
            var request = Mappings.CreateExecuteCloudScriptRequest(Const.FUNC_CAN_START_GAME);

            OnApiCallStart();
            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    OnApiCallEnd();
                    // cloud script can execute but it doesnt mean
                    // our logic did not throw something.
                    if (success.Error == null)
                    {
                        if (onSuccess != null)
                        {
                            onSuccess(success.FunctionResult.ToString());
                        }
                    }
                    else
                    {
                        OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(success.Error));
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Gets leader board data using range specified in <see cref="Const"/>.
        /// </summary>
        public void GetLeaderboardData()
        {
            var request = Mappings.CreateGetLeaderboardRequest(Const.HIGHSCORE_START_INDEX, 
                Const.HIGHSCORE_RANGE_COUNT, 
                Const.STATISTIC_NAME);

            OnApiCallStart();
            PlayFabClientAPI.GetLeaderboard(request,
            success =>
            {
                OnApiCallEnd();
                var result = success.Leaderboard.MapToModel();
                OnLeaderboardRefresh(this, new PlayfabRefreshLeaderboardsDataEventArgs(result));
            },
            failed =>
            {
                OnApiCallEnd();
                OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
            });
        }

        /// <summary>
        /// Submits player score if its greater than his previous one.
        /// </summary>
        /// <param name="scoreToSubmit"></param>
        public void SubmitHighscore(string scoreToSubmit)
        {
            var request = Mappings.CreateExecuteCloudScriptRequest(Const.FUNC_SUBMIT_HIGHSCORE, new { Highscore = scoreToSubmit });

            OnApiCallStart();
            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    OnApiCallEnd();
                    if (success.Error == null)
                    {
                        Debug.Log("Highscore submitted");
                    }
                    else
                    {
                        OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(success.Error));
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Adds coins to user profile.
        /// </summary>
        /// <param name="id">Id of session game was played in.</param>
        /// <param name="amount">Amount of coins to add.</param>
        public void AddCoins(string id, int amount)
        {
            var request = Mappings.CreateExecuteCloudScriptRequest(Const.FUNC_ADD_COINS, new { Id = id, Amount = amount });

            OnApiCallStart();
            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    OnApiCallEnd();
                    // cloud script can execute but it doesnt mean
                    // our logic did not throw something.
                    if (success.Error == null)
                    {
                        Debug.Log("Coins added");
                    }
                    else
                    {
                        OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(success.Error));
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                    Debug.LogError(failed.ToString());
                });
        }

        /// <summary>
        /// Registers new playfab user.
        /// </summary>
        /// <param name="data">Registration data.</param>
        public void RegisterUser(RegisterData data)
        {
            var request = Mappings.CreateRegisterPlayFabUserRequest(data.Username, data.Email, data.Password);

            OnApiCallStart();
            PlayFabClientAPI.RegisterPlayFabUser(request,
                success =>
                {
                    OnApiCallEnd();
                    OnUserRegistered(this, new PlayfabOnUserRegisteredEventArgs());
                },
                failed =>
                {
                    OnApiCallEnd();
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed));
                });
        }

        public void SignOut()
        {
            PlayFabClientAPI.ForgetAllCredentials();
            PlayerPrefs.DeleteKey(Const.PLAYFAB_PASSWORD);
            PlayerPrefs.DeleteKey(Const.PLAYFAB_USERNAME);
            GameManager.Inst.StartLoginState();
        }
    }
}
