using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    /// <summary>
    /// Defines PC input controller.
    /// </summary>
    public class PCInputController : IInputController
    {
        public bool HideButton { get { return Input.GetKeyDown(KeyCode.E); } }

        public bool Left { get { return Input.GetKey(KeyCode.A); } }

        public bool Right { get { return Input.GetKey(KeyCode.D); } }

        public bool Up { get { return Input.GetKey(KeyCode.W); } }

        public bool Down { get { return Input.GetKey(KeyCode.S); } }

        public bool MovementInputDetected { get { return Left || Right || Up || Down; } }
    }
}
