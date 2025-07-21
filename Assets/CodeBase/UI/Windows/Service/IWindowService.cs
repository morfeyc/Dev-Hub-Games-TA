using Cysharp.Threading.Tasks;

namespace CodeBase.UI.Windows.Service
{
  public interface IWindowService
  {
    UniTask Open(WindowId id);
    UniTask<TWindow> Open<TWindow>(WindowId id) where TWindow : WindowBase;
    void Close(WindowId id);
    void CloseAll();
    void CleanUp();
  }
}