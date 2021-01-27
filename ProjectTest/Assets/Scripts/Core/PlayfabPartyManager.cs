using Assets.Scripts.Data.Events;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Party;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class PlayfabPartyManager : SingletonBehaviour<PlayfabPartyManager>
    {
        private void Awake()
        {
            PlayFabMultiplayerManager.Get().OnNetworkJoined += OnNetworkJoined;
            PlayFabMultiplayerManager.Get().OnRemotePlayerJoined += OnRemotePlayerJoined;
            PlayFabMultiplayerManager.Get().OnRemotePlayerLeft += OnRemotePlayerLeft;
            PlayFabMultiplayerManager.Get().OnDataMessageReceived += OnDataMessageReceived;
            PlayFabMultiplayerManager.Get().OnChatMessageReceived += OnChatMessageReceived;
            PlayfabManager.Inst.OnConneectionDataRecieved += JoinRoom;
        }

        public void CreateRoom()
        {
            PlayFabMultiplayerManager.Get().CreateAndJoinNetwork();
        }

        public void JoinRoom(object sender, PlayfabOnConnectionDataRecievedEventArgs networkToJoin)
        {
            PlayFabMultiplayerManager.Get().JoinNetwork(networkToJoin.NetworkId);
        }

        private void OnLoginSuccess(LoginResult result)
        {
        }

        private void OnLoginFailure(PlayFabError error)
        {
        }

        private void OnNetworkJoined(object sender, string networkId)
        {
            PlayfabManager.Inst.UpdateUserData(
                new System.Collections.Generic.Dictionary<string, string>() { { "test", networkId } }, 
                permission: UserDataPermission.Public);
            Debug.Log(networkId);
        }

        private void OnRemotePlayerLeft(object sender, PlayFabPlayer player)
        {
        }

        private void OnRemotePlayerJoined(object sender, PlayFabPlayer player)
        {
        }

        private void OnDataMessageReceived(object sender, PlayFabPlayer from, byte[] buffer)
        {
            Debug.Log(Encoding.Default.GetString(buffer));
        }

        private void OnChatMessageReceived(object sender, PlayFabPlayer from, string message, ChatMessageType type)
        {
            Debug.Log(message);
        }

        public void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("FIREE");
                byte[] requestAsBytes = Encoding.UTF8.GetBytes("Hello (data message)");
                PlayFabMultiplayerManager.Get().SendDataMessageToAllPlayers(requestAsBytes);
            }
        }
    }
}
