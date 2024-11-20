using System;
namespace YogiGameCore.Utils
{
    public class TestStateMachine
    {
        float moveSpeed = 0;
        bool isAttacking = false;
        int enemyID = 10;
        bool isPlayerFreezed = false;
        StateMachine stateMachine;
        void Awake()
        {
            stateMachine = new StateMachine();
            var idle = new Idle();
            var walk = new Walk();
            var attack = new Attack(enemyID);
            var freeze = new Freeze();

            Func<bool> HasMoving = () => moveSpeed > 0;
            Func<bool> HasAttacking = () => isAttacking;


            stateMachine.AddAnyTransition(freeze, () => isPlayerFreezed);

            AddTr(idle, attack, () => HasAttacking());
            AddTr(idle, walk, () => HasMoving());
            AddTr(walk, attack, () => HasAttacking());
            AddTr(walk, idle, () => !HasMoving());
            AddTr(attack, walk, () => HasMoving() && !HasAttacking());
            AddTr(attack, idle, () => !HasMoving() && !HasAttacking());

            void AddTr(IState from, IState to, Func<bool> predicate) =>
                stateMachine.AddTransition(from, to, predicate);

            stateMachine.SetState(idle);
        }
        void Update()
        {
            stateMachine.Tick();
        }
        public class Idle : IState
        {
            public void OnEnter()
            {
            }

            public void OnExit()
            {
            }

            public void Tick()
            {
            }
        }
        public class Walk : IState
        {
            public void OnEnter()
            {
            }

            public void OnExit()
            {
            }

            public void Tick()
            {
            }
        }
        public class Attack : IState
        {
            private int id;
            public Attack(int targetID)
            {
                id = targetID;
            }
            public void OnEnter()
            {
            }

            public void OnExit()
            {
            }

            public void Tick()
            {
            }
        }
        public class Freeze : IState
        {
            public void OnEnter()
            {
            }

            public void OnExit()
            {
            }

            public void Tick()
            {
            }
        }
    }
}
