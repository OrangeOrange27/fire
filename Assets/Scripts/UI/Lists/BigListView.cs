using System.Collections.Generic;
using Infra;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BigListView : MonoBehaviour
    {
        private const int DEBUG_ITEM_COUNT = 1000;

        [SerializeField] private RectTransform _content;
        [SerializeField] private TMP_Text _itemPrefab; // we just store the prefab here for simplicity. In the real project we can load it dynamicly via Addressables or else
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _spacing = 10f;

        private readonly List<TMP_Text> _activeItems = new();

        private GameObjectPool<TMP_Text> _pool;
        private string[] _data;
        private float _itemHeight;
        private int _visibleCount;
        
        private int _firstVisibleIndex = -1; // force first update
        
        private void Awake()
        {
            GenerateDebugData(DEBUG_ITEM_COUNT);
        }

        private void Start()
        {
            _itemHeight = _itemPrefab.rectTransform.sizeDelta.y;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, (_itemHeight + _spacing) * _data.Length);

            _pool = new GameObjectPool<TMP_Text>(_itemPrefab, _content);
            _itemPrefab.gameObject.SetActive(false);

            _visibleCount = Mathf.CeilToInt(_scrollRect.viewport.rect.height / (_itemHeight + _spacing)) + 1;

            UpdateVisibleItems();
        }

        private void Update()
        {
            UpdateVisibleItems();
        }

        private void GenerateDebugData(int count)
        {
            _data = new string[count];
            for (var i = 0; i < count; i++)
                _data[i] = $"---------- {i} ----------";
        }

        private void UpdateVisibleItems()
        {
            var scrollY = _content.anchoredPosition.y;
            var newFirstIndex = Mathf.Clamp(Mathf.FloorToInt(scrollY / (_itemHeight + _spacing)), 0, _data.Length - 1);

            if (newFirstIndex == _firstVisibleIndex) return;

            foreach (var item in _activeItems)
                _pool.Release(item);
            _activeItems.Clear();

            _firstVisibleIndex = newFirstIndex;

            var lastIndex = Mathf.Min(_firstVisibleIndex + _visibleCount, _data.Length);
            for (var i = _firstVisibleIndex; i < lastIndex; i++)
            {
                var item = _pool.Get();
                item.text = _data[i];
                PositionItem(item.rectTransform, i);
                _activeItems.Add(item);
            }
        }

        private void PositionItem(RectTransform rect, int index)
        {
            rect.anchoredPosition = new Vector2(0, -(index * (_itemHeight + _spacing)));
        }
    }
}