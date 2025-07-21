using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.Game
{
  public class GameLoadingState : IState
  {
    private readonly ISceneLoader _sceneLoader;

    public GameLoadingState(ISceneLoader sceneLoader)
    {
      _sceneLoader = sceneLoader;
    }
    
    public async UniTask Enter()
    {
      await _sceneLoader.Load(InfrastructureAssetPath.GameLoadingScene);
    }

    public UniTask Exit()
    {
      return default;
    }
  }
}