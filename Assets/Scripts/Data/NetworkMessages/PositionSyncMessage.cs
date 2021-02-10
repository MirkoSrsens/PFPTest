using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.NetworkMessages
{
    [Serializable]
    public class PositionSyncMessage : INetworkMessage
    {
        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float RotationX { get; set; }

        public float RotationY { get; set; }

        public float RotationZ { get; set; }

        public int NetworkId { get; set; }

        public PositionSyncMessage(int networkId, float posX, float posY, float rotX, float rotY, float rotZ)
        {
            this.PositionX = posX;
            this.PositionY = posY;
            this.RotationX = rotX;
            this.RotationY = rotY;
            this.RotationZ = rotZ;
            this.NetworkId = networkId;
        }
    }
}
