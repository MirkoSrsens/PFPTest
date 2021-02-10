using Assets.Scripts.Data.NetworkMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class NetworkPositionSync : MonoBehaviour, INetworkHandler<PositionSyncMessage>
    {
        private NetworkObject header { get; set; }

        [SerializeField]
        public float TimesPerSecond = 10;

        private void Awake()
        {
            header = GetComponent<NetworkObject>();
            header.HostComponents.Add(this);
        }

        private void OnEnable()
        {
            StartCoroutine(SyncPosition());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator SyncPosition()
        {
            while(true)
            {

                var msg = new PositionSyncMessage(header.ID, transform.position.x,
                    transform.position.y,
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    transform.eulerAngles.z);

                Send(msg);

                yield return new WaitForSeconds(1f / TimesPerSecond);
            }
        }

        public void Send(PositionSyncMessage msg)
        {
            NetworkMessageHandler.Inst.Send(msg);
        }

        public void Recieve(PositionSyncMessage msg)
        {
            var position = new Vector2(msg.PositionX, msg.PositionY);
            var rotation = Quaternion.Euler(msg.RotationX, msg.RotationY, msg.RotationZ);
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
