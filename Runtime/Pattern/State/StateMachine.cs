using System.Collections.Generic;

// class IdleState : IState
// {
//     public void OnEnter() => Debug.Log("Idle 시작");
//     public void OnUpdate(float dt) { }
//     public void OnExit() => Debug.Log("Idle 종료");
// }
//
// class AttackState : IState
// {
//     public void OnEnter() => Debug.Log("Attack 시작");
//     public void OnUpdate(float dt) { }
//     public void OnExit() => Debug.Log("Attack 종료");
// }
//
// StateMachine sm = new StateMachine();
// sm.ChangeState<IdleState>();
// sm.Update(Time.deltaTime);
// sm.ChangeState<AttackState>();

namespace SongLib.Patterns.State
{
    /// <summary>
    /// 게임 상태 전환 관리용 State Machine
    /// </summary>
    public class StateMachine
    {
        private IState _currentState;
        private readonly Dictionary<System.Type, IState> _stateCache = new();

        public void ChangeState<T>() where T : IState, new()
        {
            _currentState?.OnExit();

            var type = typeof(T);
            if (!_stateCache.TryGetValue(type, out var newState))
            {
                newState = new T();
                _stateCache[type] = newState;
            }

            _currentState = newState;
            _currentState.OnEnter();
        }

        public void Update(float deltaTime)
        {
            _currentState?.OnUpdate(deltaTime);
        }
    }
}