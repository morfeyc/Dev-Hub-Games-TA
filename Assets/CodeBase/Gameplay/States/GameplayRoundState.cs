using System.Linq;
using CodeBase.Gameplay.Features.AICharacter;
using CodeBase.Gameplay.Features.AICharacter.Provider;
using CodeBase.Gameplay.Features.Avatar.Provider;
using CodeBase.Gameplay.Features.Joystick.Service;
using CodeBase.Gameplay.Features.MemeQuiz.Service;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using CodeBase.Services.StaticDataService;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Service;
using CodeBase.UI.Windows.TimerWindow;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.States
{
  public class GameplayRoundState : IState, ITickableState
  {
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly IStaticDataService _staticDataService;
    private readonly IWindowService _windowService;
    private readonly IMemeQuizService _memeQuizService;
    private readonly IAvatarProvider _avatarProvider;
    private readonly IJoystickService _joystickService;
    private readonly IAICharactersProvider _aiCharactersProvider;

    private GameplaySettings _gameplaySettings;
    private TimerWindow _timerWindow;
    private float _roundTime = float.MaxValue;

    public GameplayRoundState(SceneStateMachine sceneStateMachine,
      IStaticDataService staticDataService,
      IWindowService windowService,
      IMemeQuizService memeQuizService,
      IAvatarProvider avatarProvider,
      IJoystickService joystickService,
      IAICharactersProvider aiCharactersProvider)
    {
      _sceneStateMachine = sceneStateMachine;
      _staticDataService = staticDataService;
      _windowService = windowService;
      _memeQuizService = memeQuizService;
      _avatarProvider = avatarProvider;
      _joystickService = joystickService;
      _aiCharactersProvider = aiCharactersProvider;
    }

    public async UniTask Enter()
    {
      GetGameplaySettings();
      await SetupTimerWindow();
      
      TeleportAllToStart();
      SetRoundTime(_gameplaySettings.RoundTime);
      
      _timerWindow.Open();
      _memeQuizService.GenerateNewQuiz();
      _joystickService.EnableJoystick();
      AssignAIsToAnswers();
      _avatarProvider.Avatar.EnableMovement();
    }

    private void TeleportAllToStart()
    {
      _avatarProvider.Avatar.ResetPosition();
      foreach (AICharacter aiCharacter in _aiCharactersProvider.ActiveAICharacters) 
        aiCharacter.ResetPosition();
    }

    public void Tick()
    {
      UpdateTimer();
      if (RoundTimeIsUp())
        RoundEnded();
    }

    private bool RoundTimeIsUp() => 
      _roundTime < 0;

    public UniTask Exit()
    {
      _joystickService.DisableJoystick();
      _avatarProvider.Avatar.DisableMovement();
      return default;
    }

    private void RoundEnded()
    {
      _timerWindow.Close();
      _memeQuizService.HighlightCorrectAnswer();
      _sceneStateMachine.Enter<GameplayRoundEndState, int>(_gameplaySettings.TimeBetweenRounds).Forget();
    }

    private async UniTask SetupTimerWindow()
    {
      if (_timerWindow)
        return;
      
      _timerWindow = await _windowService.Open<TimerWindow>(WindowId.Timer);
    }

    private void GetGameplaySettings()
    {
      if(_gameplaySettings)
        return;
      
      _gameplaySettings = _staticDataService.GetSingleConfig<GameplaySettings>();
    }

    private void SetRoundTime(float time)
    {
      _roundTime = time;
      _timerWindow.TimerDisplay.SetTime(Mathf.RoundToInt(time), 1);
    }

    private void UpdateTimer()
    {
      _roundTime -= Time.deltaTime;
      if (!_timerWindow) 
        return;
      
      float time = Mathf.Clamp(_roundTime, 0f, _gameplaySettings.RoundTime);
      _timerWindow.TimerDisplay.SetTime(Mathf.RoundToInt(time), time / _gameplaySettings.RoundTime);
    }

    private void AssignAIsToAnswers()
    {
      if (_aiCharactersProvider.ActiveAICharacters.Count == 1)
      {
        _aiCharactersProvider.ActiveAICharacters[0].AssignAnswerZone(_memeQuizService.GetIncorrectAnswerZone());
        return;
      }

      int position = 0;
      foreach (AICharacter aiCharacter in _aiCharactersProvider.ActiveAICharacters.OrderBy(_ => Random.value))
      {
        if(position == _aiCharactersProvider.ActiveAICharacters.Count - 1)
          aiCharacter.AssignAnswerZone(Random.value > 0.5f 
            ? _memeQuizService.GetCorrectAnswerZone() 
            : _memeQuizService.GetIncorrectAnswerZone()
            );
        
        aiCharacter.AssignAnswerZone(position % 2 == 0 
          ? _memeQuizService.GetCorrectAnswerZone() 
          : _memeQuizService.GetIncorrectAnswerZone()
          );
        
        position++;
      }
    }
  }
}
