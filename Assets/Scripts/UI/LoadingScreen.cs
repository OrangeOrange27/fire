using UnityEngine;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private ProgressBar _progressBar;

        public void SetProgress(float progress, float maxProgress)
        {
            _progressBar.SetProgress(progress, maxProgress);
        }
    }
}