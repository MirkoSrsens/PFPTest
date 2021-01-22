using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
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
            Priority = 10;

            if(PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData += OnStatsRecieved;
            }
        }

        public override void OnEnter_State()
        {
            designController.StartTask(GetType().Name, true);

            _rigb.velocity = new Vector2(_rigb.velocity.x, 0);
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

        private void OnDestroy()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData -= OnStatsRecieved;
            }
        }

        private void OnStatsRecieved(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            if (eventArgs.Data.ContainsKey("WingFlapsStrenght"))
            {
                _force += int.Parse(eventArgs.Data["WingFlapsStrenght"].Value)/10;
            }
        }
    }
}
