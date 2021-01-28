using Assets.Scripts.Data.Events;
using Assets.Scripts.Data.NetworkMessages;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Party;
using System;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class PlayfabPartyManager : SingletonBehaviour<PlayfabPartyManager>
    {
        public bool IsHost { get; private set; }

        public bool IsOnline { get; private set; }

        public int MaxNumberOfRemoteUsers = 2;

        public int MinNumberOfRemoteUsers = 1;

        private int CurrentNumberOfUsers { get; set; }

        public PlayFabMultiplayerManager playfabManager { get; set; }

        private void Awake()
        {
            playfabManager = PlayFabMultiplayerManager.Get();
            playfabManager.OnNetworkJoined += OnNetworkJoined;
            playfabManager.OnNetworkLeft += OnNetworkLeft;
            playfabManager.OnRemotePlayerJoined += OnRemotePlayerJoined;
            playfabManager.OnRemotePlayerLeft += OnRemotePlayerLeft;
            playfabManager.OnDataMessageReceived += OnDataMessageReceived;
            playfabManager.OnChatMessageReceived += OnChatMessageReceived;
            PlayfabManager.Inst.OnConneectionDataRecieved += JoinRoom;
        }

        public void CreateRoom()
        {
            IsHost = true;
            playfabManager.CreateAndJoinNetwork();
        }

        public void JoinRoom(object sender, PlayfabOnConnectionDataRecievedEventArgs networkToJoin)
        {
            IsHost = false;
            playfabManager.JoinNetwork(networkToJoin.NetworkId);
        }

        private void OnLoginSuccess(LoginResult result)
        {
        }

        private void OnLoginFailure(PlayFabError error)
        {
        }

        private void OnNetworkLeft(object sender, string networkId)
        {
            IsOnline = true;
        }

        private void OnNetworkJoined(object sender, string networkId)
        {
            PlayfabManager.Inst.UpdateUserData(
                new System.Collections.Generic.Dictionary<string, string>() { { "test", networkId } }, 
                permission: UserDataPermission.Public);
            Debug.Log(networkId);
            IsOnline = true;
        }

        private void OnRemotePlayerLeft(object sender, PlayFabPlayer player)
        {
        }

        private void OnRemotePlayerJoined(object sender, PlayFabPlayer player)
        {
            CurrentNumberOfUsers++;

            if(CurrentNumberOfUsers == MinNumberOfRemoteUsers && IsHost)
            {
                GameManager.Inst.StartNetworkSession();
            }
        }

        private void OnDataMessageReceived(object sender, PlayFabPlayer from, byte[] buffer)
        {
            Debug.Log(Encoding.Default.GetString(buffer));
            var msg = buffer.ByteArrayToObject();
            if(msg is NotifyGameIsStarting)
            {
                NetworkingAdapter.Inst.HandleNotifyClientGameIsStarting((NotifyGameIsStarting)msg);
            }
            if (msg is SpawnMessage)
            {
                NetworkingAdapter.Inst.HandleSpawnPlayers((SpawnMessage)msg);
            }
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
                playfabManager.SendDataMessageToAllPlayers(requestAsBytes);
            }
        }
    }
}
