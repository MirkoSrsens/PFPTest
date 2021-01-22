using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ConstantMovement : State
    {
        private Rigidbody2D _rigb { get; set; }

        private int _direction { get; set; }

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _maximumDownVelocity;

        [SerializeField]
        private float _maximumUpVelocity;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            _rigb = GetComponent<Rigidbody2D>();
        }

        public override void Update_State()
        {
            if(controller.activeState == null)
            {
                controller.SwapState(this);
            }


            // Move as long as your not dead.
            if (!(controller.activeState is Death))
            {
                _rigb.velocity = new Vector2(_speed, _rigb.velocity.y);


                _direction = _rigb.velocity.y > 0 ? 1 : -1;

                if (_direction > 0 && Mathf.Abs(_rigb.velocity.y) > _maximumUpVelocity)
                {
                    _rigb.velocity = new Vector2(_speed, _maximumUpVelocity);
                }
                else if (_direction < 0 && Mathf.Abs(_rigb.velocity.y) > _maximumDownVelocity)
                {
                    _rigb.velocity = new Vector2(_speed, -_maximumUpVelocity);
                }
            }
        }

        private void OnDestroy()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData -= OnStatsRecieved;
            }
        }

        private void OnStatsRecieved(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            if (eventArgs.Data.ContainsKey("MovementSpeed"))
            {
                _speed += float.Parse(eventArgs.Data["MovementSpeed"].Value)/10;
            }
        }
    }
}
