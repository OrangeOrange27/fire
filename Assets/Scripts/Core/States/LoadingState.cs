using System;
using Cysharp.Threading.Tasks;
using Infra;
using Infra.Logging;
using Infra.StateMachine;
using UI;
using UnityEngine;

namespace Core.States
{
    public class LoadingState : IGameState
    {
        private readonly GameStateMachine _machine;
        private readonly LoadingScreen _loadingScreen;

        private const float SimulatedLoadTime = 2f;
        private const float UpdateInterval = 0.1f;

        public LoadingState(GameStateMachine machine, LoadingScreen loadingScreen)
        {
            _machine = machine;
            _loadingScreen = loadingScreen;
        }

        public async UniTask Enter()
        {
            var progress = 0f;
            var elapsed = 0f;

            var mockTask = UniTask.Delay(TimeSpan.FromSeconds(SimulatedLoadTime));
            
            while (!mockTask.Status.IsCompleted())
            {
                elapsed += UpdateInterval;
                progress = Mathf.Clamp01(elapsed / SimulatedLoadTime);

                _loadingScreen.SetProgress(progress, 1);
                LoggingFacade.Log($"Loading Progress: {progress * 100f:F1}%");

                await UniTask.Delay(TimeSpan.FromSeconds(UpdateInterval));
            }

            await mockTask;

            await AddressablesSceneLoader.LoadSceneAsync(GlobalConstants.SceneNames.GameScene);

            await _machine.Enter<GameLoopState>();
        }

        public UniTask Exit() => UniTask.CompletedTask;
    }
}