using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Enviroment
{
    public class ParalaxFollow : State
    {
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        public override void Update_State()
        {
            base.Update_State();

            if(controller.activeState == null)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(gameInformation.Player.transform.position.x, transform.position.y, transform.position.z), 0.1f*Time.deltaTime);
        }

    }
}
