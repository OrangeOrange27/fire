using Cysharp.Threading.Tasks;

namespace Infra.StateMachine
{
    public interface IGameState
    {
        UniTask Enter();
        UniTask Exit();
    }
}