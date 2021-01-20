using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
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
    public class Death : State
    {
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        private Rigidbody2D _rigb { get; set; }

        private BoxCollider2D _collider { get; set; }

        private CameraFollow cameraFollowState { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 1000;
            _rigb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            cameraFollowState = gameInformation.Camera.GetComponent<CameraFollow>();
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            StartCoroutine(StopMovingAfter());
        }

        private IEnumerator StopMovingAfter()
        {
            _collider.enabled = false;
            _rigb.AddForce(new Vector2(0, 20));
            _rigb.gravityScale = 10;
            cameraFollowState.controller.EndState(cameraFollowState);
            yield return new WaitForSeconds(2);
            _rigb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
