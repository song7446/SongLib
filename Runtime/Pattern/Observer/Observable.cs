using System;

// Observable<int> playerHp = new(100);
//
// playerHp.OnValueChanged += hp => Debug.Log($"HP: {hp}");
// playerHp.Value -= 10; // 이벤트 자동 호출


namespace SongLib.Patterns.Observer
{
    /// <summary>
    /// 단일 값 변경을 구독/통보하는 옵저버 패턴 클래스
    /// </summary>
    public class Observable<T>
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value)) return;
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        public event Action<T> OnValueChanged;

        public Observable(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}