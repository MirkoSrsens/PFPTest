using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class FlapWings : State
    {
        private Rigidbody2D _rigb { get; set; }

        [SerializeField]
        private float _force;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            _rigb = GetComponent<Rigidbody2D>();
        }

        public override void OnEnter_State()
        {
            designController.StartTask(GetType().Name, true);

            _rigb.AddForce(new Vector2(0, _force), ForceMode2D.Impulse);
            controller.EndState(this);
        }

        public override void Update_State()
        {
            base.Update_State();
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    controller.SwapState(this);
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                controller.SwapState(this);
            }
        }

        public override void OnExit_State()
        {
        }
    }
}
