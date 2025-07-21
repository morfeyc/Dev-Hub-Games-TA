using System;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine
{
  public interface IStateMachine
  {
    Type CurrentStateType { get; }
    UniTask Enter<TState>() where TState : class, IState;
    UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
    void RegisterState<TState>(TState state) where TState : IExitableState;
  }
}