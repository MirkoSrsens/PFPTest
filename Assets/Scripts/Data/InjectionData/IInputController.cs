namespace Assets.Scripts.Data.InjectionData
{
    public interface IInputController
    {
        bool MovementInputDetected { get; }

        bool Left { get; }

        bool Right { get; }

        bool Up { get; }

        bool Down { get; }
    }
}
