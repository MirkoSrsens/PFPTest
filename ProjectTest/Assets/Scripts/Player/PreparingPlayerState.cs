using General.State;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Defines state used to allow player few seconds of brake on game start.
    /// </summary>
    public class PreparingPlayerState : State
    {
        /// <summary>
        /// Gets or sets rigidbody component.
        /// </summary>
        private Rigidbody2D _rigb { get; set; }

        /// <summary>
        /// Gets or sets gravity scale value.
        /// </summary>
        private float _gravityScale { get; set; }

        /// <inheritdoc/>
        protected override void Initialization_State()
        {
            base.Initialization_State();
            _rigb = GetComponent<Rigidbody2D>();
            _gravityScale = _rigb.gravityScale;
            controller.SwapState(this);
        }

        /// <inheritdoc/>
        public override void OnEnter_State()
        {
            base.OnEnter_State();
            _rigb.gravityScale = 0;
            StartCoroutine(EndStateAfter());
        }

        /// <inheritdoc/>
        public override void WhileActive_State()
        {
            base.WhileActive_State();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                controller.EndState(this);
            }
        }

        /// <inheritdoc/>
        private IEnumerator EndStateAfter()
        {
            yield return new WaitForSeconds(2);
            controller.EndState(this);
        }

        /// <inheritdoc/>
        public override void OnExit_State()
        {
            base.OnExit_State();
            _rigb.gravityScale = _gravityScale;
        }
    }
}
