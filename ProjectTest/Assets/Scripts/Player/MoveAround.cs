using Assets.Scripts.Data.NetworkMessages;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class MoveAround : State
    {
        private Rigidbody2D rigb { get; set; }

        private NetworkObject header { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            rigb = GetComponent<Rigidbody2D>();
            header = GetComponent<NetworkObject>();
            header.HostComponents.Add(this);
            header.Refresh();
        }

        public override void Update_State()
        {
            base.Update_State();
            if(controller.activeState == null)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            rigb.velocity = new Vector2(h * 10, v * 10);
        }
    }
}
