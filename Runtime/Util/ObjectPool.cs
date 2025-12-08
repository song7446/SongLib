using System;
using System.Collections.Generic;

namespace SongLib.Utility.Collections
{
    /// <summary>
    /// GC 없는 객체 재사용을 위한 제네릭 풀 클래스
    /// </summary>
    public class ObjectPool<T> where T : class, new()
    {
        private readonly Stack<T> _pool = new();
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public int Count => _pool.Count;

        public ObjectPool(Action<T> onGet = null, Action<T> onRelease = null)
        {
            _onGet = onGet;
            _onRelease = onRelease;
        }

        public T Get()
        {
            T obj = _pool.Count > 0 ? _pool.Pop() : new T();
            _onGet?.Invoke(obj);
            return obj;
        }

        public void Release(T obj)
        {
            _onRelease?.Invoke(obj);
            _pool.Push(obj);
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}

// var pool = new ObjectPool<List<int>>(
//     onGet: list => list.Clear()
// );
//
// var list1 = pool.Get();
// list1.Add(1);
// pool.Release(list1);