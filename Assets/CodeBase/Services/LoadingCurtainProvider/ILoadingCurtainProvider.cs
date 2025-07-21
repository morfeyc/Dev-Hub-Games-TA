using Cysharp.Threading.Tasks;

namespace CodeBase.Services.LoadingCurtainProvider
{
  public interface ILoadingCurtainProvider
  {
    UniTask InitializeAsync();
    void Show();
    void Hide();
  }
}