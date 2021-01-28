using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.NetworkMessages;
using PlayFab.Party;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Data.Events.PlayfabOnGameStartedEventArgs;

namespace Assets.Scripts.Core
{
    public class NetworkingAdapter : SingletonBehaviour<NetworkingAdapter>
    {
        public GameObject playerPrefab;

        public PlayFabMultiplayerManager playfabManager { get; set; }

        private void Awake()
        {
            playfabManager = PlayFabMultiplayerManager.Get();
        }

        public void SendCatchupData()
        {

        }

        public void NotifyClientsGameIsStarting()
        {
            var msg = new NotifyGameIsStarting();
            playfabManager.SendDataMessageToAllPlayers(msg.ObjectToByteArray());
            SpawnPlayers();
        }

        public void HandleNotifyClientGameIsStarting(NotifyGameIsStarting message)
        {
            GameManager.Inst.StartNetworkSession();
        }

        private void SpawnPlayers()
        {
            Pool.Inst.NetworkSendSpawn(playerPrefab.transform, Vector2.zero, Quaternion.identity);
        }

        public void HandleSpawnPlayers(SpawnMessage msg)
        {
            Pool.Inst.NetworkRecieveSpawn(msg);
        }
    }
}
