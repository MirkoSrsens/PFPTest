using General.State;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PreparingPlayerState : State
    {
        private Rigidbody2D _rigb { get; set; }

        private float _gravityScale { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            _rigb = GetComponent<Rigidbody2D>();
            _gravityScale = _rigb.gravityScale;
            controller.SwapState(this);
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            _rigb.gravityScale = 0;
            StartCoroutine(EndStateAfter());
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                controller.EndState(this);
            }
        }

        private IEnumerator EndStateAfter()
        {
            yield return new WaitForSeconds(2);
            controller.EndState(this);
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            _rigb.gravityScale = _gravityScale;
        }
    }
}
