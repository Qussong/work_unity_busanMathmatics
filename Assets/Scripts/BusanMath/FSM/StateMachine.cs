using System;
using System.Collections.Generic;
using UnityEngine;

namespace BusanMath.FSM
{
    public class StateMachine : MonoBehaviour
    {
        private IState _currentState;
        private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
        private HashSet<Type> _initializedStates = new HashSet<Type>();

        public IState CurrentState => _currentState;

        public event Action<IState, IState> OnStateChanged;

        public void AddState<T>(T state) where T : IState
        {
            var type = typeof(T);
            if (!_states.ContainsKey(type))
            {
                _states[type] = state;
            }
        }

        public void InitializeAllStates()
        {
            foreach (var kvp in _states)
            {
                if (_initializedStates.Add(kvp.Key))
                {
                    kvp.Value.Init();
                }
            }
        }

        public void ChangeState<T>() where T : IState
        {
            var type = typeof(T);

            if (!_states.TryGetValue(type, out IState newState))
            {
                Debug.LogError($"[StateMachine] State not found: {type.Name}");
                return;
            }

            if (_currentState == newState) return;

            var oldState = _currentState;

            _currentState?.Exit();
            _currentState = newState;

            if (_initializedStates.Add(type))
            {
                _currentState.Init();
            }

            _currentState.Enter();

            OnStateChanged?.Invoke(oldState, _currentState);

            Debug.Log($"[StateMachine] {oldState?.GetType().Name ?? "None"} → {_currentState.GetType().Name}");
        }

        public T GetState<T>() where T : IState
        {
            var type = typeof(T);
            if (_states.TryGetValue(type, out IState state))
            {
                return (T)state;
            }
            return default;
        }

        public bool IsCurrentState<T>() where T : IState
        {
            return _currentState is T;
        }

        private void Update()
        {
            _currentState?.Update();
        }

        private void OnDestroy()
        {
            foreach (var state in _states.Values)
            {
                state.Dispose();
            }
        }
    }
}
