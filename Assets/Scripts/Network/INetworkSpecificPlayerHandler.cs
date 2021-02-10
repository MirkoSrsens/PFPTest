using Assets.Scripts.Data.NetworkMessages;
using PlayFab.Party;

namespace Assets.Scripts.Network
{
    public interface INetworkSpecificPlayerHandler<T> where T : INetworkMessage
    {
        void Send(T msg, PlayFabPlayer player);

        void Recieve(T msg);
    }
}
