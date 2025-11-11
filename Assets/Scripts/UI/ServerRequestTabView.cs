using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Infra.Logging;

namespace UI
{
    public class ServerRequestTabView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _requestsAmountText;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            SendEmptyRequest().Forget();
        }

        private async UniTaskVoid SendEmptyRequest()
        {
            _button.interactable = false; // prevent double clicks
            
            _requestsAmountText.text = "Updating...";

            try
            {
                var response = await Server.ServerAPI.SendEmpty();
                
                _requestsAmountText.text = response != null ? response?.RequestCount.ToString() : "0";
            }
            catch (System.Exception ex)
            {
                LoggingFacade.LogError($"Request failed: {ex}");
                _requestsAmountText.text = "Error sending request";
            }
            finally
            {
                _button.interactable = true;
            }
        }
    }
}