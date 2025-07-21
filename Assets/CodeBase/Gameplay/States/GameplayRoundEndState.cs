using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Features.AICharacter;
using CodeBase.Gameplay.Features.AICharacter.Factory;
using CodeBase.Gameplay.Features.AICharacter.Provider;
using CodeBase.Gameplay.Features.Avatar.Provider;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using Cysharp.Threading.Tasks;

namespace CodeBase.Gameplay.States
{
  public class GameplayRoundEndState : IPayloadedState<int>
  {
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly IAvatarProvider _avatarProvider;
    private readonly IAICharactersProvider _aiCharactersProvider;
    private readonly IAICharacterFactory _aICharacterFactory;

    public GameplayRoundEndState(SceneStateMachine sceneStateMachine, 
      IAvatarProvider avatarProvider, 
      IAICharactersProvider aiCharactersProvider, 
      IAICharacterFactory aICharacterFactory)
    {
      _sceneStateMachine = sceneStateMachine;
      _avatarProvider = avatarProvider;
      _aiCharactersProvider = aiCharactersProvider;
      _aICharacterFactory = aICharacterFactory;
    }

    public async UniTask Enter(int waitingTime)
    {
      var copyList = new AICharacter[_aiCharactersProvider.ActiveAICharacters.Count];
      _aiCharactersProvider.ActiveAICharacters.CopyTo(copyList, 0);

      foreach (AICharacter aiCharacter in copyList)
      {
        if(aiCharacter.IsAssignedAnswerCorrectOne())
          aiCharacter.Celebrate();
        else
          _aICharacterFactory.DespawnAICharacter(aiCharacter);
      }

      if (_avatarProvider.Avatar.ChooseCorrectAnswer())
      {
        _avatarProvider.Avatar.CelebrateRound();
        if (PlayerLastOneAlive())
        {
          _sceneStateMachine.Enter<GameplayWinState>().Forget();
          return;
        }
      }
      else
      {
        _sceneStateMachine.Enter<GameplayLoseState>().Forget();
        return;
      }

      await UniTask.Delay(TimeSpan.FromSeconds(waitingTime));
      _sceneStateMachine.Enter<GameplayRoundState>().Forget();
    }

    private bool PlayerLastOneAlive() =>
      _aiCharactersProvider.ActiveAICharacters.Count == 0;

    public UniTask Exit()
    {
      return default;
    }
  }
}