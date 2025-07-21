using CodeBase.GameLoading.States;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factory;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.GameLoading
{
  public class GameLoadingSceneBootstrapper : IInitializable
  {
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly StatesFactory _statesFactory;

    public GameLoadingSceneBootstrapper(SceneStateMachine sceneStateMachine, StatesFactory statesFactory)
    {
      _sceneStateMachine = sceneStateMachine;
      _statesFactory = statesFactory;
    }

    public void Initialize()
    {
      DLogger.Message(DSenders.Initialization)
        .WithText("Start GameLoading initialisation")
        .Log();

      // More states to prepare game-loop, like ads, network, analytics and more
      _sceneStateMachine.RegisterState(_statesFactory.Create<FinishGameLoadingState>());

      DLogger.Message(DSenders.Initialization)
        .WithText("Finish GameLoading initialisation")
        .Log();
      
      _sceneStateMachine.Enter<FinishGameLoadingState>().Forget();
    }
  }
}