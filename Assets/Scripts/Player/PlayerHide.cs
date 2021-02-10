using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerHide : State
    {
        [SerializeField]
        private float speed;

        private Collider2D hideObject { get; set; }

        private Rigidbody2D rigb { get; set; }

        private bool isUp { get; set; }

        private bool isRight { get; set; }

        private Vector2 pointOfContact { get; set; }

        [InjectDiContainter]
        private IInputController input { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            rigb = GetComponent<Rigidbody2D>();
            Priority = 20;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            transform.position = pointOfContact;
            isUp = pointOfContact.y <= transform.position.y;
            isRight = pointOfContact.x <= transform.position.x;
        }

        public override void Update_State()
        {
            if (input.HideButton && controller.activeState == this)
            {
                controller.EndState(this);
            }
            else if (hideObject != null && input.HideButton)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
        }

        public override void WhileActiveFixed_State()
        {
            float directionX = (input.Left ? -1 : 0) + (input.Right ? 1 : 0);
            float directionY = (input.Up ? 1 : 0) + (input.Down ? -1 : 0);

            if (directionX != 0)
            {
                transform.localScale = new Vector3(directionX, 1, 1);
            }

            if (directionY != 0 && directionX != 0)
            {
                directionX *= 0.7f;
                directionY *= 0.7f;
            }


            rigb.velocity = new Vector3(directionX * speed, directionY * speed);
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.tag == "Hide" && controller.activeState != this)
            {
                hideObject = collision;
                pointOfContact = collision.ClosestPoint(transform.position);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision == hideObject)
            {
                controller.EndState(this);
                hideObject = null;
            }
        }
    }
}
