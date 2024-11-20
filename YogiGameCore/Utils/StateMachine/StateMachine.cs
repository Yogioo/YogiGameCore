using System;
using System.Collections.Generic;

namespace YogiGameCore.Utils
{
    /// <summary>
    /// 更轻量化的状态机, 使用更多的委托的方式实现而不是使用更多的状态类
    /// 如果更复杂,需要使用状态类请使用 FSMSystem 类与他的 FSMState 基类协同进行
    /// </summary>
    public class StateMachine
    {
        private IState _currentState;
        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();
        private static List<Transition> EmptyTransitions = new List<Transition>(0);
        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (!_transitions.TryGetValue(from.GetType(), out var transitions))
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }
            transitions.Add(new Transition(to, predicate));
        }
        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }
        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);
            _currentState?.Tick();
        }
        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition.Invoke())
                    return transition;
            foreach (var transition in _currentTransitions)
                if (transition.Condition.Invoke())
                    return transition;
            return null;
        }
        public void SetState(IState state)
        {
            if (state == _currentState)
                return;
            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            _currentState.OnEnter();
        }
        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }

            public Transition(IState to, Func<bool> predicate)
            {
                To = to;
                this.Condition = predicate;
            }
        }
    }

    public interface IState
    {
        void Tick();
        void OnEnter();
        void OnExit();
    }
}