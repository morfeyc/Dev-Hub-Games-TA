using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.Game
{
  public class GameplayState : IState
  {
    private readonly ISceneLoader _sceneLoader;

    public GameplayState(ISceneLoader sceneLoader)
    {
      _sceneLoader = sceneLoader;
    }

    public async UniTask Enter()
    {
      await _sceneLoader.Load(InfrastructureAssetPath.GameplayScene);
    }

    public async UniTask Exit()
    {
    }
  }
}