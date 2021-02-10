using Assets.Scripts.Data.NetworkMessages;

namespace Assets.Scripts.Network
{
    public interface INetworkHandler<T> where T: INetworkMessage
    {
        void Send(T msg);

        void Recieve(T msg);
    }
}
