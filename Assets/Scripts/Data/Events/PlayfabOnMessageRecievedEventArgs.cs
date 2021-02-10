using Assets.Scripts.Data.NetworkMessages;
using System;

namespace Assets.Scripts.Data.Events
{
    public class PlayfabOnMessageRecievedEventArgs : EventArgs
    {

        public delegate void OnNetworkMessageRecievedEventHandler(INetworkMessage msg);
    }
}
