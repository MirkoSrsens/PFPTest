using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Defines stat of camera that follows player.
    /// </summary>
    public class CameraFollow : State
    {
        /// <summary>
        /// Gets or sets injected <see cref="IGameInformation"/> data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        /// <summary>
        /// Defines offset from player in negative X axis.
        /// </summary>
        [SerializeField]
        private float _offsetFromplayer;

        /// <inheritdoc/>
        protected override void Initialization_State()
        {
            base.Initialization_State();
            controller.SwapState(this);
        }

        /// <inheritdoc/>
        public override void WhileActive_State()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(gameInformation.Player.transform.position.x + _offsetFromplayer, transform.position.y, transform.position.z), 100);
        }
    }
}
