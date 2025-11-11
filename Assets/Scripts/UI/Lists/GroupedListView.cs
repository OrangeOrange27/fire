using System.Collections.Generic;
using Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Infra;

namespace UI
{
    public class GroupedListView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private TMP_Text _itemPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _spacing = 10f;

        private readonly List<TMP_Text> _activeItems = new();

        private List<ItemData> _data;
        private float _itemHeight;
        private int _visibleCount;

        private GameObjectPool<TMP_Text> _pool;

        private int _firstVisibleIndex = -1; // force initial update

        private void Awake()
        {
            GenerateGroupedData();
        }

        private void Start()
        {
            _itemHeight = _itemPrefab.rectTransform.sizeDelta.y;
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, (_itemHeight + _spacing) * _data.Count);

            _pool = new GameObjectPool<TMP_Text>(_itemPrefab, _content);
            _itemPrefab.gameObject.SetActive(false);

            _visibleCount = Mathf.CeilToInt(_scrollRect.viewport.rect.height / (_itemHeight + _spacing)) + 1;

            UpdateVisibleItems();
        }

        private void Update()
        {
            UpdateVisibleItems();
        }

        /// <summary>
        /// Generates a sample grouped list: Green=Uncommon, Purple=Epic, Gold=Legendary
        /// Adjust counts here as needed
        /// </summary>
        private void GenerateGroupedData()
        {
            var counts = new Dictionary<Rarity, int>
            {
                { Rarity.Uncommon, 10 },
                { Rarity.Epic, 6 },
                { Rarity.Legendary, 2 }
            };

            _data = new List<ItemData>();

            while (counts[Rarity.Uncommon] + counts[Rarity.Epic] + counts[Rarity.Legendary] > 0)
            {
                foreach (var rarity in new[] { Rarity.Uncommon, Rarity.Epic, Rarity.Legendary })
                {
                    if (counts[rarity] > 0)
                    {
                        _data.Add(new ItemData { Rarity = rarity });
                        counts[rarity]--;
                    }
                }
            }
        }

        private void UpdateVisibleItems()
        {
            var scrollY = _content.anchoredPosition.y;
            var newFirstIndex = Mathf.Clamp(Mathf.FloorToInt(scrollY / (_itemHeight + _spacing)), 0, _data.Count - 1);

            if (newFirstIndex == _firstVisibleIndex) return;

            foreach (var item in _activeItems)
                _pool.Release(item);
            _activeItems.Clear();

            _firstVisibleIndex = newFirstIndex;

            var lastIndex = Mathf.Min(_firstVisibleIndex + _visibleCount, _data.Count);
            for (var i = _firstVisibleIndex; i < lastIndex; i++)
            {
                var item = _pool.Get();
                item.text = _data[i].Rarity.ToString();
                item.color = GetColorForRarity(_data[i].Rarity);
                PositionItem(item.rectTransform, i);
                _activeItems.Add(item);
            }
        }

        private void PositionItem(RectTransform rect, int index)
        {
            rect.anchoredPosition = new Vector2(0, -(index * (_itemHeight + _spacing)));
        }

        private static Color GetColorForRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Uncommon => Color.green,
                Rarity.Epic => Color.purple,
                Rarity.Legendary => Color.gold,
                _ => Color.white
            };
        }
    }
}