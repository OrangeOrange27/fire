using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private Slider _slider;
    
    public void SetProgress(float progress, float maxProgress)
    {
        var realProgress = progress / maxProgress;
        _slider.value = realProgress;
        _progressText.text = $"{Mathf.RoundToInt(realProgress * 100)}%";
    } 
}
