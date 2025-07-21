using Zenject;

namespace CodeBase.UI.Factory
{
  public class UIFactoryInstaller : Installer<UIFactoryInstaller>
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesTo<UIFactory>().AsSingle().NonLazy();
    }
  }
}