using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using CodeBase.Services.LoadingCurtainProvider;
using CodeBase.Services.StaticDataService;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.Game
{
  public class GameBootstrapState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IAssetProvider _assetProvider;
    private readonly IStaticDataService _staticDataService;
    private readonly ILoadingCurtainProvider _loadingCurtainProvider;

    public GameBootstrapState(GameStateMachine gameStateMachine, 
      IAssetProvider assetProvider, 
      IStaticDataService staticDataService,
      ILoadingCurtainProvider loadingCurtainProvider)
    {
      _staticDataService = staticDataService;
      _gameStateMachine = gameStateMachine;
      _assetProvider = assetProvider;
      _loadingCurtainProvider = loadingCurtainProvider;
    }

    public async UniTask Enter()
    {
      await InitServices();
      
      _gameStateMachine.Enter<GameLoadingState>().Forget();
    }

    public UniTask Exit()
    {
      return default;
    }

    private async Task InitServices()
    {
      await _assetProvider.InitializeAsync();
      await _staticDataService.LoadAll();
      await _loadingCurtainProvider.InitializeAsync();
    }
  }
}