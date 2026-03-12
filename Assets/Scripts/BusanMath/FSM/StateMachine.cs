using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
    private HashSet<Type> _initializedStates = new HashSet<Type>(); // Init 호출 여부 추적

    public IState CurrentState => _currentState;

    // 상태 전환 이벤트
    public event Action<IState, IState> OnStateChanged; // oldState, newState

    /// <summary>
    /// 상태 등록
    /// </summary>
    public void AddState<T>(T state) where T : IState
    {
        var type = typeof(T);
        if (!_states.ContainsKey(type))
        {
            _states[type] = state;
        }
    }

    /// <summary>
    /// 상태 전환 : Exit → Init(최초 1회) → Enter
    /// </summary>
    public void ChangeState<T>() where T : IState
    {
        var type = typeof(T);

        if (!_states.TryGetValue(type, out IState newState))
        {
            Debug.LogError($"[StateMachine] State not found: {type.Name}");
            return;
        }

        // 같은 상태면 무시
        if (_currentState == newState) return;

        var oldState = _currentState;

        // 현재 상태 Exit
        _currentState?.Exit();
        _currentState = newState;

        // 최초 진입 시 Init 호출 (1회만)
        if (_initializedStates.Add(type))
        {
            _currentState.Init();
        }

        _currentState.Enter();

        // 외부에 상태 변경 알림
        OnStateChanged?.Invoke(oldState, _currentState);

        Debug.Log($"[StateMachine] {oldState?.GetType().Name ?? "None"} → {_currentState.GetType().Name}");
    }

    /// <summary>
    /// 특정 타입 상태 가져오기
    /// </summary>
    public T GetState<T>() where T : IState
    {
        var type = typeof(T);
        if (_states.TryGetValue(type, out IState state))
        {
            return (T)state;
        }
        return default;
    }

    /// <summary>
    /// 현재 상태가 특정 타입인지 확인
    /// </summary>
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
        // 모든 상태의 이벤트 구독 해제
        foreach (var state in _states.Values)
        {
            state.Dispose();
        }
    }
}
