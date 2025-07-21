using CodeBase.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.UI.Factory
{
  public interface IUIFactory
  {
    UniTask<WindowBase> CreateWindow(AssetReference assetReference);
    UniTask<TComponent> CreateAndPlaceUIElement<TComponent>(AssetReference assetReference) where TComponent : MonoBehaviour;
  }
}