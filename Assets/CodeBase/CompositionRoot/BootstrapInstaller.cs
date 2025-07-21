using CodeBase.Infrastructure;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Services.InputService;
using CodeBase.Services.LoadingCurtainProvider;
using CodeBase.Services.StaticDataService;
using Zenject;

namespace CodeBase.CompositionRoot
{
  public class BootstrapInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesTo<AssetProvider>().AsSingle();
      Container.BindInterfacesTo<StaticDataService>().AsSingle();
      Container.BindInterfacesTo<SceneLoader>().AsSingle();
      Container.BindInterfacesTo<InputService>().AsSingle();
      Container.BindInterfacesTo<LoadingCurtainProvider>().AsSingle();

      GameStateMachineInstaller.Install(Container);
    }
  }
}