using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CameraFollow : State
    {
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            controller.SwapState(this);
        }

        public override void WhileActive_State()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(gameInformation.Player.transform.position.x, transform.position.y, transform.position.z), 100);
        }
    }
}
