using CodeBase.Gameplay.Features.AICharacter.Factory;
using CodeBase.Gameplay.Features.AICharacter.Provider;
using CodeBase.Gameplay.Features.Avatar.Factory;
using CodeBase.Gameplay.Features.Avatar.Provider;
using CodeBase.Gameplay.Features.Joystick.Service;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factory;
using CodeBase.Services.PrefabPoolingService;
using CodeBase.UI;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay
{
  public class GameplaySceneInstaller : MonoInstaller
  {
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<SceneStateMachine>().AsSingle().WithArguments("Gameplay");
      Container.BindInterfacesAndSelfTo<StatesFactory>().AsSingle();
      Container.BindInterfacesTo<GameplaySceneBootstrapper>().AsSingle().NonLazy();
      
      Container.Bind<CinemachineCamera>().FromInstance(_cinemachineCamera).AsSingle();
      Container.BindInterfacesTo<PrefabPoolingService>().AsSingle();
      
      Container.BindInterfacesTo<JoystickService>().AsSingle();
      Container.BindInterfacesTo<AvatarFactory>().AsSingle();
      Container.BindInterfacesTo<AvatarProvider>().AsSingle();

      Container.BindInterfacesTo<AICharactersProvider>().AsSingle();
      Container.BindInterfacesTo<AICharacterFactory>().AsSingle();

      UIInstaller.Install(Container);
    }
  }
}