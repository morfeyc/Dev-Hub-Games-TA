using CodeBase.Gameplay.Features.Joystick;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.UI.Windows;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace CodeBase.UI.Factory
{
  public class UIFactory : IUIFactory, IInitializable
  {
    private readonly IInstantiator _instantiator;
    private readonly IAssetProvider _assetProvider;
    
    private Transform _uiRoot;

    public UIFactory(IInstantiator instantiator, IAssetProvider assetProvider)
    {
      _assetProvider = assetProvider;
      _instantiator = instantiator;
    }

    public void Initialize()
    {
      GameObject uiRoot = CreateUIRoot();
      _uiRoot = uiRoot.transform;
    }

    private GameObject CreateUIRoot()
    {
      GameObject uiRoot = _instantiator.InstantiatePrefabResource(InfrastructureAssetPath.UIRootKey);
      DLogger.Message(DSenders.UI)
        .WithText("UI Root".White())
        .WithText(" Created")
        .Log();

      return uiRoot;
    }

    public async UniTask<WindowBase> CreateWindow(AssetReference assetReference)
    {
      GameObject windowPrefab = await _assetProvider.Load<GameObject>(assetReference);
      return _instantiator.InstantiatePrefabForComponent<WindowBase>(windowPrefab, parentTransform: _uiRoot);
    }
    
    public async UniTask<TComponent> CreateAndPlaceUIElement<TComponent>(AssetReference assetReference) where TComponent : MonoBehaviour
    {
      GameObject uiElementPrefab = await _assetProvider.Load<GameObject>(assetReference);
      return _instantiator.InstantiatePrefabForComponent<TComponent>(uiElementPrefab);
    }
  }
}