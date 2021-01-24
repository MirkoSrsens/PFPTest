using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    /// <summary>
    /// Defines input for mobile platforms.
    /// </summary>
    public class MobileInputController : IInputController
    {
        public bool IsKeyPressed
        {
            get
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
