using CodeBase.Infrastructure.StateMachine.StateTypes;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Service;
using Cysharp.Threading.Tasks;

namespace CodeBase.Gameplay.States
{
  public class GameplayLoseState : IState
  {
    private readonly IWindowService _windowService;

    public GameplayLoseState(IWindowService windowService)
    {
      _windowService = windowService;
    }
    
    public UniTask Enter()
    {
      _windowService.CloseAll();
      _windowService.Open(WindowId.LoseScreen);
      return default;
    }

    public UniTask Exit()
    {
      return default;
    }
  }
}