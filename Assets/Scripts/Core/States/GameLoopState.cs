using Cysharp.Threading.Tasks;
using Infra.StateMachine;
using UnityEngine;

namespace Core.States
{
    public class GameLoopState : IGameState
    {
        public UniTask Enter()
        {
            Debug.Log("GameLoop started");
            return UniTask.CompletedTask;
        }

        public UniTask Exit() => UniTask.CompletedTask;
    }
}