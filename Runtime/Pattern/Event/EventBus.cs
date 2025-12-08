using System;
using System.Collections.Generic;

// // 이벤트 정의
// public struct PlayerDamaged { public int damage; }
//
// // 구독
// EventBus.Subscribe<PlayerDamaged>(OnDamaged);
//
// // 발행
// EventBus.Publish(new PlayerDamaged { damage = 10 });
//
// // 콜백
// void OnDamaged(PlayerDamaged e) => Debug.Log($"플레이어 피해: {e.damage}");

namespace SongLib.Patterns.Event
{
    /// <summary>
    /// 게임 전반의 이벤트 송신/수신을 중개하는 EventBus.
    /// Observer 패턴 기반.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _eventTable = new();

        public static void Subscribe<T>(Action<T> listener)
        {
            var type = typeof(T);
            if (_eventTable.TryGetValue(type, out var existingDelegate))
                _eventTable[type] = Delegate.Combine(existingDelegate, listener);
            else
                _eventTable[type] = listener;
        }

        public static void Unsubscribe<T>(Action<T> listener)
        {
            var type = typeof(T);
            if (_eventTable.TryGetValue(type, out var existingDelegate))
            {
                var current = Delegate.Remove(existingDelegate, listener);
                if (current == null) _eventTable.Remove(type);
                else _eventTable[type] = current;
            }
        }

        public static void Publish<T>(T eventData)
        {
            if (_eventTable.TryGetValue(typeof(T), out var del))
                (del as Action<T>)?.Invoke(eventData);
        }

        public static void Clear() => _eventTable.Clear();
    }
}

