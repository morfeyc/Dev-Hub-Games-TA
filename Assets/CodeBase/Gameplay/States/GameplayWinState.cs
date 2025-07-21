using CodeBase.Gameplay.Features.Avatar.Provider;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Service;
using Cysharp.Threading.Tasks;

namespace CodeBase.Gameplay.States
{
  public class GameplayWinState : IState
  {
    private readonly IWindowService _windowService;
    private readonly IAvatarProvider _avatarProvider;

    public GameplayWinState(IWindowService windowService, IAvatarProvider avatarProvider)
    {
      _windowService = windowService;
      _avatarProvider = avatarProvider;
    }

    public UniTask Enter()
    {
      _windowService.CloseAll();
      _windowService.Open(WindowId.WinScreen);
      _avatarProvider.Avatar.CelebrateVictory();
      
      return default;
    }

    public UniTask Exit()
    {
      return default;
    }
  }
}