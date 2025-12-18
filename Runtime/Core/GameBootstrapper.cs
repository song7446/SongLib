using System;
using System.Collections.Generic;

namespace SongLib.Core.Bootstrap
{
    public class Bootstrapper
    {
        private readonly Queue<IGameInitializer> _queue = new();
        private Action _onFinished;

        public Bootstrapper Add(IGameInitializer initializer)
        {
            if (initializer != null)
                _queue.Enqueue(initializer);

            return this;
        }

        public void Run(Action onFinished = null)
        {
            _onFinished = onFinished;
            RunNext();
        }

        private void RunNext()
        {
            if (_queue.Count == 0)
            {
                _onFinished?.Invoke();
                return;
            }

            var current = _queue.Dequeue();
            current.Initialize(RunNext);
        }
    }
}