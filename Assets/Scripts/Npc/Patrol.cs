using General.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Npc
{
    public class Patrol : State
    {
        [SerializeField]
        private Transform[] points;

        [SerializeField]
        private float stopTime;

        [SerializeField]
        private float movementSpeed;

        private bool continuePatrol { get; set; }

        private int currentIndex { get; set; }

        private Stack<Vector2> travelRotue { get; set; }

        private const float offsetFromPoint = 0.1f;

        private Vector2 nextPoint { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            continuePatrol = true;
            Priority = 10;
            currentIndex = 0;
            nextPoint = default(Vector2);
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            travelRotue = Pathfinding.Inst.GetPath(transform, points[currentIndex]);


            currentIndex = ++currentIndex % points.Length;
        }

        public override void Update_State()
        {
            if(continuePatrol)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            if(travelRotue != null 
                && (nextPoint == default(Vector2)
                || Vector2.Distance(transform.position, nextPoint) < offsetFromPoint))
            {
                if(travelRotue.Count == 0)
                {
                    controller.EndState(this);
                    return;
                }
                nextPoint = travelRotue.Pop();
            }

        }

        public override void WhileActiveFixed_State()
        {
            if (nextPoint != default(Vector2))
            {
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, movementSpeed * Time.fixedDeltaTime);
            }
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            travelRotue = null;
        }

    }
}
