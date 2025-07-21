using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.Game
{
  public class MainMenuState : IState
  {
    private readonly ISceneLoader _sceneLoader;

    public MainMenuState(ISceneLoader sceneLoader)
    {
      _sceneLoader = sceneLoader;
    }
    
    public async UniTask Enter()
    {
      await _sceneLoader.Load(InfrastructureAssetPath.MainMenuScene);
    }

    public UniTask Exit()
    {
      return default;
    }
  }
}