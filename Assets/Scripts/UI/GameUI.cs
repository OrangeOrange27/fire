using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button _bigListTabButton;
        [SerializeField] private Button _groupedListTabButton;
        [SerializeField] private Button _serverRequestTabButton;

        [SerializeField] private RectTransform _bigListTab;
        [SerializeField] private RectTransform _groupedListTab;
        [SerializeField] private RectTransform _serverRequestTab;

        private CancellationTokenSource _animationCts;
        private int _currentTabIndex = 0;

        private RectTransform[] Tabs => new[]
        {
            _bigListTab,
            _groupedListTab,
            _serverRequestTab
        };

        private void Awake()
        {
            _bigListTabButton.onClick.AddListener(() => ShowTab(0));
            _groupedListTabButton.onClick.AddListener(() => ShowTab(1));
            _serverRequestTabButton.onClick.AddListener(() => ShowTab(2));
        }

        private void ShowTab(int i)
        {
            var currentTab = Tabs[_currentTabIndex];
            var nextTab = Tabs[i];

            if (currentTab == nextTab)
                return;

            _animationCts?.Cancel();
            _animationCts = new CancellationTokenSource();

            ChangeTabWithAnimation(currentTab, nextTab, _animationCts.Token).Forget();
            
            _currentTabIndex = i;
        }

        private async UniTask ChangeTabWithAnimation(RectTransform currentTab, RectTransform nextTab,
            CancellationToken token)
        {
            const float duration = 0.3f;

            nextTab.gameObject.SetActive(true);

            var width = ((RectTransform)nextTab.parent).rect.width;

            nextTab.anchoredPosition = new Vector2(width, 0);

            var elapsed = 0f;

            var startPosCurrent = currentTab != null ? currentTab.anchoredPosition : Vector2.zero;
            var endPosCurrent = currentTab != null ? new Vector2(-width, 0) : Vector2.zero;

            var startPosNext = nextTab.anchoredPosition;
            var endPosNext = Vector2.zero;

            while (elapsed < duration)
            {
                if (token.IsCancellationRequested)
                    return;

                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var smoothT = Mathf.SmoothStep(0f, 1f, t);

                if (currentTab != null)
                    currentTab.anchoredPosition = Vector2.Lerp(startPosCurrent, endPosCurrent, smoothT);

                nextTab.anchoredPosition = Vector2.Lerp(startPosNext, endPosNext, smoothT);

                await UniTask.Yield();
            }

            nextTab.anchoredPosition = endPosNext;
            if (currentTab != null)
            {
                currentTab.anchoredPosition = endPosCurrent;
                currentTab.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _bigListTabButton.onClick.RemoveAllListeners();
            _groupedListTabButton.onClick.RemoveAllListeners();
            _serverRequestTabButton.onClick.RemoveAllListeners();
        }
    }
}