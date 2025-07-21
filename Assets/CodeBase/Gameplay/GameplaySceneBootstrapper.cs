using CodeBase.Gameplay.States;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factory;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Gameplay
{
  public class GameplaySceneBootstrapper : IInitializable
  {
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly StatesFactory _statesFactory;

    public GameplaySceneBootstrapper(SceneStateMachine sceneStateMachine, StatesFactory statesFactory)
    {
      _sceneStateMachine = sceneStateMachine;
      _statesFactory = statesFactory;
    }

    public void Initialize()
    {
      DLogger.Message(DSenders.Initialization)
        .WithText("Start Gameplay initialisation")
        .Log();
      
      _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayInitialisationState>());
      _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());
      _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayRoundState>());
      _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayRoundEndState>());
      _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayLoseState>());
      _sceneStateMachine.RegisterState(_statesFactory.Create<GameplayWinState>());

      DLogger.Message(DSenders.Initialization)
        .WithText("Finish Gameplay initialisation")
        .Log();
      
      _sceneStateMachine.Enter<GameplayInitialisationState>().Forget();
    }
  }
}