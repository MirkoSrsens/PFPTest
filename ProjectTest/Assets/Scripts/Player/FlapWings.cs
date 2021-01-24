using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Defines player stat for moving upwards on mouse click.
    /// </summary>
    public class FlapWings : State
    {
        /// <summary>
        /// Gets or sets injected object used for input implementation.
        /// </summary>
        [InjectDiContainter]
        private IInputController input { get; set; }

        /// <summary>
        /// Defines upwards force strenght.
        /// </summary>
        [SerializeField]
        private float _force;

        /// <summary>
        /// Gets or sets rigidbody component.
        /// </summary>
        private Rigidbody2D _rigb { get; set; }

        /// <inheritdoc/>
        protected override void Initialization_State()
        {
            base.Initialization_State();
            _rigb = GetComponent<Rigidbody2D>();
            Priority = 10;

            if(PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData += OnStatsRecieved;
            }
        }

        /// <inheritdoc/>
        public override void OnEnter_State()
        {
            designController.StartTask(GetType().Name, true);

            _rigb.velocity = new Vector2(_rigb.velocity.x, 0);
            _rigb.AddForce(new Vector2(0, _force), ForceMode2D.Impulse);
            controller.EndState(this);
        }

        /// <inheritdoc/>
        public override void Update_State()
        {
            base.Update_State();

            if(input.IsKeyPressed)
            {
                controller.SwapState(this);
            }
        }

        /// <inheritdoc/>
        public override void OnExit_State()
        {
        }

        private void OnDestroy()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData -= OnStatsRecieved;
            }
        }

        /// <summary>
        /// Updates force strength on stats refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnStatsRecieved(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            if (eventArgs.Data.ContainsKey(Const.STAT_WING_FLAPS_STRENGHT))
            {
                _force += int.Parse(eventArgs.Data[Const.STAT_WING_FLAPS_STRENGHT].Value)/10;
            }
        }
    }
}
