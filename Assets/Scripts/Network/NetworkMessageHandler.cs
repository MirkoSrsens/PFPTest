using Assets.Scripts.Core;
using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.NetworkMessages;
using PlayFab.Party;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class NetworkMessageHandler : SingletonBehaviour<NetworkMessageHandler>, INetworkHandler<PositionSyncMessage>, INetworkSpecificPlayerHandler<AssignPlayerMessage>, INetworkHandler<NotifyGameIsStarting>, INetworkHandler<SpawnMessage>
    {
        public NetworkObject playerPrefab;

        public PlayFabMultiplayerManager playfabManager { get; set; }

        private void Awake()
        {
            playfabManager = PlayFabMultiplayerManager.Get();
            PlayfabPartyManager.Inst.OnMessageRecieved += HandlerRecieve;
        }

        private void HandlerRecieve(INetworkMessage msg)
        {
            Recieve(msg as NotifyGameIsStarting);
            Recieve(msg as SpawnMessage);
            Recieve(msg as AssignPlayerMessage);
            Recieve(msg as PositionSyncMessage);
        }

        public void Send(NotifyGameIsStarting msg)
        {
            playfabManager.SendDataMessageToAllPlayers(msg.ObjectToByteArray());

            // Spawn players

            // Host
            var idH = Pool.Inst.NetworkSendSpawn(playerPrefab, new Vector2(10, 10), Quaternion.identity);
            if (Pool.Inst.NetworkRegisteredObjects.ContainsKey(idH))
            {
                Pool.Inst.NetworkRegisteredObjects[idH].ControlledLocally = true;
            }

            // Clients 
            foreach (var player in playfabManager.RemotePlayers)
            {
                var idC = Pool.Inst.NetworkSendSpawn(playerPrefab, new Vector2(10, 10), Quaternion.identity);
                Send(new AssignPlayerMessage(idC), player);
                Pool.Inst.NetworkRegisteredObjects[idC].ControlledLocally = false;
            }
        }

        public void Recieve(NotifyGameIsStarting msg)
        {
            if (msg == null) return;

            GameManager.Inst.StartNetworkSession();
        }

        public void Send(SpawnMessage msg)
        {
            playfabManager.SendDataMessageToAllPlayers(msg.ObjectToByteArray());
        }

        public void Recieve(SpawnMessage msg)
        {
            if (msg == null) return;

            Pool.Inst.NetworkRecieveSpawn(msg);
        }

        public void Send(AssignPlayerMessage msg, PlayFabPlayer player)
        {
            playfabManager._SendDataMessage(msg.ObjectToByteArray(), new[] { player }, DeliveryOption.Guaranteed);
        }

        public void Recieve(AssignPlayerMessage msg)
        {
            if (msg == null) return;

            if(Pool.Inst.NetworkRegisteredObjects.ContainsKey(msg.ObjectID))
            {
                Pool.Inst.NetworkRegisteredObjects[msg.ObjectID].ControlledLocally = true;
            }
            Debug.Log("Controller by this player");
        }

        public void Send(PositionSyncMessage msg)
        {
            playfabManager.SendDataMessageToAllPlayers(msg.ObjectToByteArray());
        }

        public void Recieve(PositionSyncMessage msg)
        {
            if (msg == null) return;

            if (Pool.Inst.NetworkRegisteredObjects.ContainsKey(msg.NetworkId))
            {
                Pool.Inst.NetworkRegisteredObjects[msg.NetworkId].GetComponent<NetworkPositionSync>().Recieve(msg);
                Debug.Log("Position synced");
            }
        }
    }
}
