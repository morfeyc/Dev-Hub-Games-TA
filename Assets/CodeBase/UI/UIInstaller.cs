using CodeBase.UI.Factory;
using CodeBase.UI.Windows.Service;
using Zenject;

namespace CodeBase.UI
{
  public class UIInstaller : Installer<UIInstaller>
  {
    public override void InstallBindings()
    {
      UIFactoryInstaller.Install(Container);
      
      Container.BindInterfacesTo<WindowService>().AsSingle();
    }
  }
}