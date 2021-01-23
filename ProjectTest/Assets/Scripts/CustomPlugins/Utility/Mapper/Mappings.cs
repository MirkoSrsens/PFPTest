using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.Scripts.CustomPlugins.Utility
{
    /// <summary>
    /// Contains login of mapping and creating models.
    /// </summary>
    public static class Mappings
    {
        /// <summary>
        /// Maps <see cref="List{PlayerLeaderboardEntry}"/> to <see cref="List{LeaderboardData}"/>
        /// </summary>
        /// <param name="data">The data to map.</param>
        /// <returns></returns>
        public static List<LeaderboardData> MapToModel(this List<PlayerLeaderboardEntry> data)
        {
            var result = new List<LeaderboardData>();

            foreach (var item in data)
            {
                var name = string.IsNullOrEmpty(item.DisplayName) ? item.PlayFabId : item.DisplayName;
                result.Add(new LeaderboardData(name, item.StatValue, item.Position));
            }

            return result;
        }

        /// <summary>
        /// Creates new <see cref="LoginWithEmailAddressRequest"/>.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>Return new <see cref="LoginWithEmailAddressRequest"./></returns>
        public static LoginWithEmailAddressRequest CreateEmailRequest(string email, string password)
        {
            return new LoginWithEmailAddressRequest()
            {
                Email = email,
                Password = password,
                TitleId = PlayFabSettings.TitleId,

                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
                {
                    GetPlayerProfile = true,
                    ProfileConstraints = new PlayerProfileViewConstraints()
                    {
                        ShowDisplayName = true,
                        ShowContactEmailAddresses = true
                    }
                }
            };
        }

        /// <summary>
        /// Creates new <see cref="GetAccountInfoRequest"/>.
        /// </summary>
        /// <param name="playfabUserID">Optional parameter, will use default if not provided.</param>
        /// <returns>Return new <see cref="GetAccountInfoRequest"/>.</returns>
        public static GetAccountInfoRequest CreateAccountInfoRequest(string playfabUserID = null)
        {
            return new GetAccountInfoRequest()
            {
                PlayFabId = playfabUserID
            };
        }

        /// <summary>
        /// Creates new <see cref="GetCatalogItemsRequest"/>.
        /// </summary>
        /// <param name="catalogVersion">Optional parameter, will use default if not provided.</param>
        /// <returns>Return new <see cref="GetCatalogItemsRequest"/></returns>
        public static GetCatalogItemsRequest CreateGetCatalogItemRequest(string catalogVersion = null)
        {
            return new GetCatalogItemsRequest()
            {
                CatalogVersion = catalogVersion
            };
        }

        /// <summary>
        /// Creates new <see cref="GetUserInventoryRequest"/>.
        /// </summary>
        /// <param name="customTags">Optional parameter, will use default if not provided.</param>
        /// <returns>Returns new <see cref="GetCatalogItemsRequest"/>.</returns>
        public static GetUserInventoryRequest CreateGetUserInventoryRequest(Dictionary<string, string> customTags = null)
        {
            return new GetUserInventoryRequest()
            {
                CustomTags = customTags
            };
        }

        /// <summary>
        /// Creates new <see cref="GetUserDataRequest"/>.
        /// </summary>
        /// <param name="keys">Optional parameter, will use default if not provided.</param>
        /// <returns>The new <see cref="GetUserDataRequest"/>.</returns>
        public static GetUserDataRequest CreateGetUserDataRequest(List<string> keys = null)
        {
            return new GetUserDataRequest()
            {
                Keys = keys
            };
        }

        /// <summary>
        /// Creates new <see cref="ExecuteCloudScriptRequest"/>.
        /// </summary>
        /// <param name="functionName">The function name.</param>
        /// <param name="functionParam">The parameters. Optional/Depending on the function type and input parameters.</param>
        /// <returns></returns>
        public static ExecuteCloudScriptRequest CreateExecuteCloudScriptRequest(string functionName, object functionParam = null)
        {
            return new ExecuteCloudScriptRequest()
            {
                FunctionName = functionName,
                FunctionParameter = functionParam
            };
        }

        /// <summary>
        /// Creates new <see cref="GetLeaderboardRequest"/>
        /// </summary>
        /// <param name="startPosition">Index of count start.</param>
        /// <param name="maxResultsCount">Number of results to get.</param>
        /// <param name="statisticName">Name of leaderboard.</param>
        /// <returns>The new <see cref="GetLeaderboardRequest"/>. </returns>
        public static GetLeaderboardRequest CreateGetLeaderboardRequest(int startPosition, int maxResultsCount, string statisticName)
        {
            return new GetLeaderboardRequest()
            {
                StartPosition = startPosition,
                MaxResultsCount = maxResultsCount,
                StatisticName = statisticName
            };
        }

        /// <summary>
        /// Creates new <see cref="RegisterPlayFabUserRequest"/>
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static RegisterPlayFabUserRequest CreateRegisterPlayFabUserRequest(string username, string email, string password)
        {
            return new RegisterPlayFabUserRequest()
            {
                DisplayName = username,
                Username = username,
                Email = email,
                Password = password
            };
        }
    }
}
