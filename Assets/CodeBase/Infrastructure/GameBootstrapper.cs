using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factory;
using CodeBase.Infrastructure.StateMachine.Game;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class GameBootstrapper : IInitializable
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly StatesFactory _statesFactory;

    public GameBootstrapper(GameStateMachine gameStateMachine, StatesFactory statesFactory)
    {
      _gameStateMachine = gameStateMachine;
      _statesFactory = statesFactory;
    }

    public void Initialize()
    {
      DLogger.Message(DSenders.Initialization)
        .WithText("Start GameBootstrapper initialisation")
        .Log();

      _gameStateMachine.RegisterState(_statesFactory.Create<GameBootstrapState>());
      _gameStateMachine.RegisterState(_statesFactory.Create<GameLoadingState>());
      _gameStateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
      _gameStateMachine.RegisterState(_statesFactory.Create<GameplayState>());
      
      DLogger.Message(DSenders.Initialization)
        .WithText("GameBootstrapper initialisation finished")
        .Log();

      _gameStateMachine.Enter<GameBootstrapState>().Forget();
    }
  }
}