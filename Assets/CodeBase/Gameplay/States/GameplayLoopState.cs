using CodeBase.Gameplay.Features.Joystick.Service;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using CodeBase.Services.LoadingCurtainProvider;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Service;
using CodeBase.UI.Windows.TimerWindow;
using Cysharp.Threading.Tasks;

namespace CodeBase.Gameplay.States
{
  public class GameplayLoopState : IState
  {
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly IJoystickService _joystickService;
    private readonly IWindowService _windowService;
    private readonly ILoadingCurtainProvider _loadingCurtainProvider;

    public GameplayLoopState(SceneStateMachine sceneStateMachine, 
      ILoadingCurtainProvider loadingCurtainProvider,
      IJoystickService joystickService,
      IWindowService windowService)
    {
      _sceneStateMachine = sceneStateMachine;
      _loadingCurtainProvider = loadingCurtainProvider;
      _joystickService = joystickService;
      _windowService = windowService;
    }

    public async UniTask Enter()
    {
      _loadingCurtainProvider.Hide();
      _joystickService.EnableJoystick();
      
      await _windowService.Open<TimerWindow>(WindowId.Timer);
      _windowService.Close(WindowId.Timer);
      
      _sceneStateMachine.Enter<GameplayRoundState>().Forget();
    }

    public UniTask Exit()
    {
      return default;
    }

  }
}