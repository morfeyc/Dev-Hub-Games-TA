using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Game;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using Cysharp.Threading.Tasks;

namespace CodeBase.GameLoading.States
{
  public class FinishGameLoadingState : IState
  {
    private readonly GameStateMachine _gameStateMachine;

    public FinishGameLoadingState(GameStateMachine gameStateMachine)
    {
      _gameStateMachine = gameStateMachine;
    }
    
    public UniTask Enter()
    {
      _gameStateMachine.Enter<MainMenuState>().Forget();
      return default;
    }

    public UniTask Exit()
    {
      return default;
    }
  }
}