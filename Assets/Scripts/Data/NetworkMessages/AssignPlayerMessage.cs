using System;

namespace Assets.Scripts.Data.NetworkMessages
{
    [Serializable]
    public class AssignPlayerMessage : INetworkMessage
    {
        public int ObjectID { get; set; }


        public AssignPlayerMessage(int id)
        {
            this.ObjectID = id;
        }
    }
}
