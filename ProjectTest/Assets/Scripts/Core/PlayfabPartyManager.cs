using Assets.Scripts.Data.Events;
using Assets.Scripts.Data.NetworkMessages;
using PlayFab.ClientModels;
using PlayFab.Party;
using System.Text;
using UnityEngine;
using static Assets.Scripts.Data.Events.PlayfabOnMessageRecievedEventArgs;

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

        public event OnNetworkMessageRecievedEventHandler OnMessageRecieved;

        private void Awake()
        {
            playfabManager = PlayFabMultiplayerManager.Get();
            playfabManager.OnNetworkJoined += OnNetworkJoined;
            playfabManager.OnNetworkLeft += OnNetworkLeft;
            playfabManager.OnRemotePlayerJoined += OnRemotePlayerJoined;
            playfabManager.OnRemotePlayerLeft += OnRemotePlayerLeft;
            playfabManager.OnDataMessageReceived += OnDataMessageReceived;
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

        private void OnNetworkLeft(object sender, string networkId)
        {
            IsOnline = false;
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
            var msg = (INetworkMessage)buffer.ByteArrayToObject();

            OnMessageRecieved?.Invoke(msg);
        }
    }
}
