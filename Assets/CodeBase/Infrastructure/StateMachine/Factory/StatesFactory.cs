using CodeBase.Infrastructure.StateMachine.StateTypes;
using Zenject;

namespace CodeBase.Infrastructure.StateMachine.Factory
{
  public class StatesFactory : IStatesFactory
  {
    private readonly IInstantiator _instantiator;

    public StatesFactory(IInstantiator instantiator) => 
      _instantiator = instantiator;

    public TState Create<TState>() where TState : IExitableState => 
      _instantiator.Instantiate<TState>();
  }
}