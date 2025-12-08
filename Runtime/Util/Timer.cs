using System;
using UnityEngine;

namespace SongLib.Utility.Time
{
    /// <summary>
    /// 특정 시간 이후에 콜백을 실행하는 타이머 클래스.
    /// Update 루프 없이 독립적으로 작동 가능.
    /// </summary>
    public class Timer
    {
        private float _duration;
        private float _elapsed;
        private Action _onComplete;
        private bool _isRunning;

        public bool IsRunning => _isRunning;

        public Timer(float duration, Action onComplete)
        {
            _duration = duration;
            _onComplete = onComplete;
        }

        public void Start()
        {
            _elapsed = 0f;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Update(float deltaTime)
        {
            if (!_isRunning) return;

            _elapsed += deltaTime;
            if (_elapsed >= _duration)
            {
                _isRunning = false;
                _onComplete?.Invoke();
            }
        }
    }
}