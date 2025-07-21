using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Game;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
  public class GameStateSwitchButton : MonoBehaviour
  {
    public enum TargetStates
    {
      None = 0,
      MainMenu = 1,
      Gameplay = 2,
    }

    [SerializeField] private TargetStates _targetState = 0;
    [SerializeField] private Button _button;

    private GameStateMachine _gameStateMachine;
    private bool _switched;

    [Inject]
    private void Construct(GameStateMachine gameStateMachine)
    {
      _gameStateMachine = gameStateMachine;
    }

    private void OnEnable() =>
      _button.onClick.AddListener(OnClick);

    private void OnDisable() =>
      _button.onClick.RemoveListener(OnClick);

    private void OnClick()
    {
      _button.onClick.RemoveListener(OnClick);

      switch (_targetState)
      {
        case TargetStates.MainMenu:
          _gameStateMachine.Enter<MainMenuState>().Forget();
          break;
        case TargetStates.Gameplay:
          _gameStateMachine.Enter<GameplayState>().Forget();
          break;
        default:
          DLogger.Message(DSenders.SceneData)
            .WithText("Button: Not valid option")
            .WithFormat(DebugFormat.Exception)
            .Log();
          break;
      }
    }
  }
}