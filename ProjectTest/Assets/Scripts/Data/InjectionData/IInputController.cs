namespace Assets.Scripts.Data.InjectionData
{
    public interface IInputController
    {
        /// <summary>
        /// Gets value signaling if specific key is being pressed.
        /// </summary>
        bool IsKeyPressed { get; }
    }
}
