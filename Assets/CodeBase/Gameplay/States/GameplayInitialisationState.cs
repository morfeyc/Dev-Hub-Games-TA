using System.Threading.Tasks;
using CodeBase.Gameplay.Features.AICharacter.Factory;
using CodeBase.Gameplay.Features.Avatar.Factory;
using CodeBase.Gameplay.Features.Avatar.Provider;
using CodeBase.Gameplay.Features.Joystick.Service;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using CodeBase.Services.LoadingCurtainProvider;
using CodeBase.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using Avatar = CodeBase.Gameplay.Features.Avatar.Avatar;

namespace CodeBase.Gameplay.States
{
  public class GameplayInitialisationState : IState
  {
    private readonly SceneStateMachine _sceneStateMachine;
    private readonly ILoadingCurtainProvider _loadingCurtainProvider;
    private readonly IStaticDataService _staticDataService;
    private readonly IJoystickService _joystickService;
    private readonly IAvatarFactory _avatarFactory;
    private readonly IAvatarProvider _avatarProvider;
    private readonly CinemachineCamera _cinemachineCamera;
    private readonly IAICharacterFactory _aiCharacterFactory;
    private GameplaySettings _gameplaySettings;

    public GameplayInitialisationState(SceneStateMachine sceneStateMachine,
      ILoadingCurtainProvider loadingCurtainProvider,
      IStaticDataService staticDataService,
      IJoystickService joystickService, 
      IAvatarFactory avatarFactory,
      IAvatarProvider avatarProvider,
      CinemachineCamera cinemachineCamera,
      IAICharacterFactory aiCharacterFactory)
    {
      _sceneStateMachine = sceneStateMachine;
      _loadingCurtainProvider = loadingCurtainProvider;
      _staticDataService = staticDataService;
      _joystickService = joystickService;
      _avatarFactory = avatarFactory;
      _avatarProvider = avatarProvider;
      _cinemachineCamera = cinemachineCamera;
      _aiCharacterFactory = aiCharacterFactory;
    }

    public async UniTask Enter()
    {
      _loadingCurtainProvider.Show();
      _gameplaySettings = _staticDataService.GetSingleConfig<GameplaySettings>();

      await _joystickService.InitializeJoystick();
      await SetupAvatar();
      await SetupAICharacters();

      _sceneStateMachine.Enter<GameplayLoopState>().Forget();
    }

    public UniTask Exit()
    {
      return default;
    }

    private async UniTask SetupAvatar()
    {
      Avatar avatar = await _avatarFactory.CreateAvatar(_gameplaySettings.SpawnPosition, Quaternion.identity);
      _avatarProvider.SetAvatar(avatar);
      _cinemachineCamera.Follow = avatar.transform;
      _cinemachineCamera.LookAt = avatar.transform;
    }

    private async UniTask SetupAICharacters()
    {
      await _aiCharacterFactory.InitializeAsync();
      
      for (int i = 0; i < _gameplaySettings.NumberOfAICharacters; i++) 
        _aiCharacterFactory.CreateAICharacter(_gameplaySettings.SpawnPosition);
    }
  }
}