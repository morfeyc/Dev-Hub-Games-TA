using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factory;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class GameStateMachineInstaller : Installer<GameStateMachineInstaller>
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<GameBootstrapper>().AsSingle().NonLazy();
      Container.Bind<StatesFactory>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
    }
  }
}