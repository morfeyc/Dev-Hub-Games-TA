using CodeBase.Infrastructure.StateMachine.StateTypes;

namespace CodeBase.Infrastructure.StateMachine.Factory
{
  public interface IStatesFactory
  {
    TState Create<TState>() where TState : IExitableState;
  }
}