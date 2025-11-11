using Cysharp.Threading.Tasks;
using Infra.StateMachine;
using Server;
using UnityEngine.AddressableAssets;

namespace Core.States
{
    public class InitializationState : IGameState
    {
        private readonly GameStateMachine _machine;

        public InitializationState(GameStateMachine machine) => _machine = machine;

        public async UniTask Enter()
        {
            await Addressables.InitializeAsync().Task;

            await ServerAPI.LoginAsync();
            
            await _machine.Enter<LoadingState>();
        }

        public UniTask Exit() => UniTask.CompletedTask;
    }
}