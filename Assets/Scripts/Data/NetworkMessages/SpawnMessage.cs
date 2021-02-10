using System;

namespace Assets.Scripts.Data.NetworkMessages
{
    [Serializable]
    public class SpawnMessage : INetworkMessage
    {
        public string PrefabName { get; set; }
        
        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float RotationX { get; set; }

        public float RotationY { get; set; }

        public float RotationZ { get; set; }

        public int NetworkId { get; set; }

        public SpawnMessage(int networkId, string prefabName, float posX, float posY, float rotX, float rotY, float rotZ)
        {
            this.PrefabName = prefabName;
            this.PositionX = posX;
            this.PositionY = posY;
            this.RotationX = rotX;
            this.RotationY = rotY;
            this.RotationZ = rotZ;
            this.NetworkId = networkId;
        }
    }
}
