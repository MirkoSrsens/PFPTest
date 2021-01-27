using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using System;
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
        public static GetAccountInfoRequest CreateAccountInfoRequest(string playfabUserID = null, string username = null, string displayName = null)
        {
            return new GetAccountInfoRequest()
            {
                PlayFabId = playfabUserID,
                Username = username,
                TitleDisplayName = displayName
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
        public static GetUserDataRequest CreateGetUserDataRequest(List<string> keys = null, string playfabId = null)
        {
            return new GetUserDataRequest()
            {
                Keys = keys,
                PlayFabId = playfabId
            };
        }

        /// <summary>
        /// Creates new <see cref="UpdateUserDataRequest"/>.
        /// </summary>
        /// <param name="keys">Optional parameter, will use default if not provided.</param>
        /// <returns>The new <see cref="UpdateUserDataRequest"/>.</returns>
        public static UpdateUserDataRequest CreateUpdateUserDataRequest(Dictionary<string,string> add = null, List<string> delete = null, UserDataPermission permission = UserDataPermission.Private)
        {
            return new UpdateUserDataRequest()
            {
                Data = add,
                KeysToRemove = delete,
                Permission = permission
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

        /// <summary>
        /// Creates new <see cref="CreateMatchmakingTicketRequest"/>
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>Returns new <see cref="CreateMatchmakingTicketRequest"/></returns>
        public static CreateMatchmakingTicketRequest CreateMatchmakingTicketRequest(string id, string type, string queueName)
        {
            return new CreateMatchmakingTicketRequest()
            {
                Creator = new MatchmakingPlayer()
                {
                    Entity = new PlayFab.MultiplayerModels.EntityKey
                    {
                        Id = id,
                        Type = type
                    },
                    Attributes = new MatchmakingPlayerAttributes
                    {
                        DataObject = new { Skill = 0 }
                    }
                },
                GiveUpAfterSeconds = 60,
                QueueName = queueName
            };
        }

        /// <summary>
        /// Creates new <see cref="GetMatchmakingTicketRequest"/>
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>Returns new <see cref="GetMatchmakingTicketRequest"/></returns>
        public static GetMatchmakingTicketRequest CreateGetMatchmakingTicketRequest(string ticketId, string queueName)
        {
            return new GetMatchmakingTicketRequest()
            {
                TicketId = ticketId,
                QueueName = queueName
            };
        }

        /// <summary>
        /// Creates new <see cref="GetMatchRequest"/>
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>Returns new <see cref="GetMatchRequest"/></returns>
        public static GetMatchRequest CreateGetMatchRequest(string matchId, string queueName)
        {
            return new GetMatchRequest()
            {
                MatchId = matchId,
                QueueName = queueName
            };
        }


        /// <summary>
        /// Creates new <see cref="CancelMatchmakingTicketRequest"/>
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns>Returns new <see cref="CancelMatchmakingTicketRequest"/></returns>
        public static CancelMatchmakingTicketRequest CancelMatchmakingTicket(string ticketId, string queueName)
        {
            return new CancelMatchmakingTicketRequest()
            {
                TicketId = ticketId,
                QueueName = queueName
            };
        }
    }
}
