using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.Scripts.CustomPlugins.Utility
{
    public static class Mappings
    {
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

        public static GetAccountInfoRequest CreateAccountInfoRequest(string playfabUserID)
        {
            return new GetAccountInfoRequest()
            {
                PlayFabId = playfabUserID
            };
        }

        public static GetCatalogItemsRequest CreateGetCatalogItemRequest(string catalogVersion = null)
        {
            return new GetCatalogItemsRequest()
            {
                CatalogVersion = catalogVersion
            };
        }

        public static GetUserInventoryRequest CreateGetUserInventoryRequest(Dictionary<string, string> customTags = null)
        {
            return new GetUserInventoryRequest()
            {
                CustomTags = customTags
            };
        }

        public static GetUserDataRequest CreateGetUserDataRequest(List<string> keys = null)
        {
            return new GetUserDataRequest()
            {
                Keys = keys
            };
        }

        public static ExecuteCloudScriptRequest CreateExecuteCloudScriptRequest(string functionName, object functionParam = null)
        {
            return new ExecuteCloudScriptRequest()
            {
                FunctionName = functionName,
                FunctionParameter = functionParam
            };
        }

        public static GetLeaderboardRequest CreateGetLeaderboardRequest(int startPosition, int maxResultsCount, string statisticName)
        {
            return new GetLeaderboardRequest()
            {
                StartPosition = startPosition,
                MaxResultsCount = maxResultsCount,
                StatisticName = statisticName
            };
        }

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
