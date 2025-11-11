using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Infra.StateMachine
{
    public class GameStateMachine
    {
        private Dictionary<Type, IGameState> _states;
        private IGameState _current;

        public void RegisterStates(params IGameState[] states)
        {
            _states = states.ToDictionary(s => s.GetType());
        }

        public async UniTask Enter<T>() where T : IGameState
        {
            if (_current != null)
                await _current.Exit();

            _current = _states[typeof(T)];
            await _current.Enter();
        }
    }
}