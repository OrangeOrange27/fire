using System.Collections.Generic;
using UnityEngine;

namespace Infra
{
    public class GameObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Stack<T> _available = new();

        public GameObjectPool(T prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public T Get()
        {
            var item = _available.Count > 0 ? _available.Pop() : Object.Instantiate(_prefab, _parent);
            item.gameObject.SetActive(true);
            return item;
        }

        public void Release(T item)
        {
            item.gameObject.SetActive(false);
            _available.Push(item);
        }
    }
}