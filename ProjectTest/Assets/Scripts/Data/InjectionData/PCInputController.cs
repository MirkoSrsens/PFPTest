using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    /// <summary>
    /// Defines PC input controller.
    /// </summary>
    public class PCInputController : IInputController
    {
        public bool IsKeyPressed
        {
            get
            {
                return Input.GetKeyDown(KeyCode.Space);
            }
        }
    }
}
