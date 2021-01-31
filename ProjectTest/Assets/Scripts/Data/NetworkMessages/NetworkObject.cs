using Assets.Scripts.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.NetworkMessages
{
    public class NetworkObject : MonoBehaviour
    {
        public int ID { get; set; }

        private bool _controlledLocally;
        public bool ControlledLocally
        {
            get
            {
                return _controlledLocally;
            }
            set
            {
                _controlledLocally = value;
                Refresh();
            }
        }

        public List<Behaviour> HostComponents { get; set; } = new List<Behaviour>();

        public void Awake()
        {
            _controlledLocally = PlayfabPartyManager.Inst.IsHost;
        }

        public void Refresh()
        {
            foreach(var comp in HostComponents)
            {
                comp.enabled = ControlledLocally;
            }
        }

    }
}
