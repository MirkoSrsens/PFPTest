using Assets.Scripts.Core;
using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Death : State
    {
        /// <summary>
        /// Gets or sets injection of <see cref="IGameInformation"/> object instance.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        /// <summary>
        /// Gets or sets rigidbody component.
        /// </summary>
        private Rigidbody2D _rigb { get; set; }

        /// <summary>
        /// Gets or sets collider component of object.
        /// </summary>
        private BoxCollider2D _collider { get; set; }

        /// <summary>
        /// Gets or sets camera follow state from <see cref="GameInformation.Camera"> object.
        /// </summary>
        private CameraFollow cameraFollowState { get; set; }

        /// <inheritdoc/>
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 1000;
            _rigb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            cameraFollowState = gameInformation.Camera.GetComponent<CameraFollow>();
        }

        /// <inheritdoc/>
        public override void OnEnter_State()
        {
            base.OnEnter_State();
            StartCoroutine(StopMovingAfter());
        }

        /// <summary>
        /// Coroutine to make death sequence more entertaining.
        /// </summary>
        /// <returns>Returns yields for enumeration.</returns>
        private IEnumerator StopMovingAfter()
        {
            _collider.enabled = false;
            _rigb.AddForce(new Vector2(0, 20));
            _rigb.gravityScale = 10;
            cameraFollowState.controller.EndState(cameraFollowState);
            yield return new WaitForSeconds(1f);
            GameManager.Inst.OnGameLose();
            yield return new WaitForSeconds(2f);
            _rigb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
