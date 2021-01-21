using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.Events;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using static Assets.Scripts.Data.Events.PlayfabCatalogItemsEventArgs;
using static Assets.Scripts.Data.Events.PlayfabErrorHandlingEventArgs;
using static Assets.Scripts.Data.Events.PlayfabRefreshCurrencyEventArgs;
using static Assets.Scripts.Data.Events.PlayfabRefreshLeaderboardsDataEventArgs;
using static Assets.Scripts.Data.Events.PlayfabUserInfoEventArgs;
using static Assets.Scripts.Data.Events.PlayfabUserInventoryEventArgs;
using static Assets.Scripts.Data.Events.PlayfabUserReadonlyDataEventArgs;

namespace Assets.Scripts.Core
{
    public class PlayfabManager : SingletonBehaviour<PlayfabManager>
    {
        private const string PLAYFAB_USERNAME = "PLAYFAB_USERNAME";

        private const string PLAYFAB_PASSWORD = "PLAYFAB_PASSWORD";

        /// <summary>
        /// Every time we request to change currency fire sequence of elements that are affected by that.
        /// </summary>
        public event PlayfabRefreshCurrencyEventHandler RefreshCurrencyDataEvent;

        /// <summary>
        /// Can be hard coded in methods but if we have change user name feature this might come in handy.
        /// </summary>
        public event PlayfabUserInfoEventHandler RefreshUserDetailsData;

        /// <summary>
        /// When we get catalog items do something with them on UI part. Left space for some platform specific subscriptions
        /// special deals etc.
        /// </summary>
        public event PlayfabCatalogItemsEventHandler RefreshCatalogItems;

        /// <summary>
        /// When we get items we should display then on user UI so he can do whatever with them.
        /// </summary>
        public event PlayfabUserInventoryEventHandler RefreshPlayerInventory;

        /// <summary>
        /// Used when we get response on get user readonly data.
        /// </summary>
        public event PlayfabUserReadonlyDataEventHandler RefreshUserReadonlyData;

        /// <summary>
        /// Used for error handling.
        /// </summary>
        public event PlayfabErrorHandlingEventHandler OnErrorEvent;

        /// <summary>
        /// Used for error handling.
        /// </summary>
        public event PlayfabRefreshLeaderboardsDataEventHandler OnLeaderboardRefresh;

        public string PlayfabCurrentUserID { get; private set; }

        public bool CheckIfLoginIsCached()
        {
            return PlayerPrefs.HasKey(PLAYFAB_USERNAME) && PlayerPrefs.HasKey(PLAYFAB_PASSWORD);
        }

        // LOGIN LOGIC

        /// <summary>
        /// Cold login means when we use cached data instead of user manually entering it.
        /// </summary>
        /// <param name="success">On success perform this</param>
        /// <param name="failed">On failed perform this.</param>
        internal void ColdLogin(Action<LoginResult> success, Action<PlayFabError> failed)
        {
            var username = Security.Decrypt(PlayerPrefs.GetString(PLAYFAB_USERNAME));
            var password = Security.Decrypt(PlayerPrefs.GetString(PLAYFAB_PASSWORD));

            PerformLogin(username, password, success, failed);
        }

        public void PerformLogin(string username, string password, Action<LoginResult> success = null, Action<PlayFabError> failed = null)
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

            PlayFabClientAPI.LoginWithPlayFab(loginRequest,
                succ =>
                {
                    Debug.Log("Login with username was successful");
                    PlayfabCurrentUserID = succ.PlayFabId;

                    PlayerPrefs.SetString(PLAYFAB_USERNAME, Security.Encrypt(username));
                    PlayerPrefs.SetString(PLAYFAB_PASSWORD, Security.Encrypt(password));

                    if (success != null)
                    {
                        success(succ);
                    }
                },
                err =>
                {
                    Debug.LogError(err.ToString());

                    if (err.Error == PlayFabErrorCode.AccountNotFound || err.Error == PlayFabErrorCode.InvalidParams)
                    {
                        // Assume user name might be email actually. Don't trigger fail since it will be handled
                        // by login with email method.
                        PerformLoginWithEmail(username, password, success, failed);
                        return;
                    }

                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(err.ToString()));
                    if (failed != null)
                    {
                        failed(err);
                    }
                });
        }

        private void PerformLoginWithEmail(string email, string password, Action<LoginResult> success, Action<PlayFabError> failed)
        {
            var loginRequest = new LoginWithEmailAddressRequest()
            {
                Email = email,
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

            PlayFabClientAPI.LoginWithEmailAddress(loginRequest,
                succ =>
                {
                    Debug.Log("Login with email was successful");
                    PlayfabCurrentUserID = succ.PlayFabId;

                    PlayerPrefs.SetString(PLAYFAB_USERNAME, Security.Encrypt(email));
                    PlayerPrefs.SetString(PLAYFAB_PASSWORD, Security.Encrypt(password));

                    if (success != null)
                    {
                        success(succ);
                    }
                },
                err =>
                {
                    Debug.LogError(err.ToString());
                    OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(err.ToString()));

                    if (failed == null)
                    {
                        failed(err);
                    }
                });
        }

        public void GetUserData()
        {
            var request = new GetAccountInfoRequest()
            {
                PlayFabId = PlayfabCurrentUserID,
            };

            PlayFabClientAPI.GetAccountInfo(request,
                success =>
                {
                    var userInfo = new PlayfabUserInfoEventArgs()
                    {
                        Username = success.AccountInfo.Username
                    };

                    RefreshUserDetailsData(this, userInfo);
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }

        public void GetCurrencyData()
        {
            var request = new GetUserInventoryRequest(); ;

            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            success =>
            {
                var eventData = new PlayfabRefreshCurrencyEventArgs(success.VirtualCurrency["EG"],
                    success.VirtualCurrency["GL"],
                    success.VirtualCurrency["DM"]);

                RefreshCurrencyDataEvent(this, eventData);
            },
            failed =>
            {

            });
        }

        public void GetCatalogItems()
        {
            var request = new GetCatalogItemsRequest();

            PlayFabClientAPI.GetCatalogItems(request,
                success =>
                {
                    RefreshCatalogItems(this, new PlayfabCatalogItemsEventArgs(success.Catalog));
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }

        public void GetUserInventory()
        {
            var request = new GetUserInventoryRequest();
            PlayFabClientAPI.GetUserInventory(request,
                success =>
                {
                    var inverntoryData = new PlayfabUserInventoryEventArgs(success.Inventory);

                    RefreshPlayerInventory(this, inverntoryData);
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }

        public void GetUserReadonlyData()
        {
            var request = new GetUserDataRequest();

            PlayFabClientAPI.GetUserReadOnlyData(request,
                success =>
                {
                    var eventArgs = new PlayfabUserReadonlyDataEventArgs(success.Data);

                    RefreshUserReadonlyData(this, eventArgs);
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }

        public void UpgradeStat(string statToUpgrade, Action successCallback)
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpgradeStat",
                FunctionParameter = new { Stat = statToUpgrade }
            };

            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    if (success.Error != null)
                    {
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
                    Debug.LogError(failed.ToString());
                });
        }

        public void UserItem(string itemId)
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "UseItem",
                FunctionParameter = new { ItemId = itemId }
            };

            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    // cloud script can execute but it doesnt mean
                    // our logic did not throw something.
                    if (success.Error == null)
                    {
                        // Just try to get it again refresh will then be performed.
                        GetUserInventory();
                    }
                    else
                    {
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }

        public void BuyItem(string itemId, string currencySelected)
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "BuyItem",
                FunctionParameter = new { ItemId = itemId, currencyType = currencySelected }
            };

            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    // cloud script can execute but it doesnt mean
                    // our logic did not throw something.
                    if (success.Error == null)
                    {
                        Debug.Log("Item bought :D");
                    }
                    else
                    {
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }

        public void StartGameRequest(Action onSuccess)
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "CanStartGame"
            };

            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    // cloud script can execute but it doesnt mean
                    // our logic did not throw something.
                    if (success.Error == null)
                    {
                        if (onSuccess != null)
                        {
                            onSuccess();
                        }
                    }
                    else
                    {
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }

        public void GetLeaderboardData()
        {
            var request = new GetLeaderboardRequest()
            {
                StartPosition = 0,
                MaxResultsCount = 10,
                StatisticName = "Highscore"

            };

            PlayFabClientAPI.GetLeaderboard(request,
            success =>
            {
                var result = success.Leaderboard.MapToModel();
                OnLeaderboardRefresh(this, new PlayfabRefreshLeaderboardsDataEventArgs(result));
            },
            failed =>
            {
                OnErrorEvent(this, new PlayfabErrorHandlingEventArgs(failed.ToString()));
            });
        }

        public void SubmitHighscore(string scoreToSubmit)
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "SubmitHighscore",
                FunctionParameter = new { Highscore = scoreToSubmit }
            };

            PlayFabClientAPI.ExecuteCloudScript(request,
                success =>
                {
                    // cloud script can execute but it doesnt mean
                    // our logic did not throw something.
                    if (success.Error == null)
                    {
                        Debug.Log("Highscore submitted");
                    }
                    else
                    {
                        Debug.LogError(success.Error.ToString());
                    }
                },
                failed =>
                {
                    Debug.LogError(failed.ToString());
                });
        }
    }
}
