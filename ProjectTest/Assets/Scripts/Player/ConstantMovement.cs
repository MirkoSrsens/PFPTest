using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Defines state for player that performs constant movement on X axis.
    /// </summary>
    public class ConstantMovement : State
    {
        /// <summary>
        /// Defines movement speed on x axis.
        /// </summary>
        [SerializeField]
        private float _speed;

        /// <summary>
        /// Defines maximum downwards gravity velocity.
        /// </summary>
        [SerializeField]
        private float _maximumDownVelocity;

        /// <summary>
        /// Defines maximum upwards velocity.
        /// </summary>
        [SerializeField]
        private float _maximumUpVelocity;

        /// <summary>
        /// Gets or sets rigidbody component.
        /// </summary>
        private Rigidbody2D _rigb { get; set; }

        /// <summary>
        /// Gets or sets direction component.
        /// </summary>
        private int _direction { get; set; }

        /// <inheritdoc/>
        protected override void Initialization_State()
        {
            base.Initialization_State();
            _rigb = GetComponent<Rigidbody2D>();
        }

        /// <inheritdoc/>
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
            if (eventArgs.Data.ContainsKey(Const.STAT_MOVEMENT_SPEED))
            {
                _speed += float.Parse(eventArgs.Data[Const.STAT_MOVEMENT_SPEED].Value)/10;
            }
        }
    }
}
