using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using UnityEngine;

public class PlayerMovement : State
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rigb { get; set; }

    [InjectDiContainter]
    private IInputController input { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        rigb = GetComponent<Rigidbody2D>();
        Priority = 10;
    }


    public override void Update_State()
    {
        if(input.MovementInputDetected)
        {
            controller.SwapState(this);
        }
    }

    public override void WhileActiveFixed_State()
    {
        base.WhileActiveFixed_State();
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

        if(directionX == 0 && directionY == 0)
        {
            controller.EndState(this);
        }
    }
}
