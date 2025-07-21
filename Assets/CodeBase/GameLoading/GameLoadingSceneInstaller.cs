using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factory;
using Zenject;

namespace CodeBase.GameLoading
{
  public class GameLoadingSceneInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<SceneStateMachine>().AsSingle().WithArguments("GameLoading");
      Container.BindInterfacesAndSelfTo<StatesFactory>().AsSingle();
      Container.BindInterfacesTo<GameLoadingSceneBootstrapper>().AsSingle().NonLazy();
    }
  }
}