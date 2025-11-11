using Core.States;
using Infra.StateMachine;
using UI;
using UnityEngine;

namespace Core
{
    // improvised simple DI

    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;

        private async void Start()
        {
            var machine = new GameStateMachine();
            
            machine.RegisterStates(
                new InitializationState(machine),
                new LoadingState(machine, _loadingScreen),
                new GameLoopState()
            );
            
            await machine.Enter<InitializationState>();
        }
    }
}