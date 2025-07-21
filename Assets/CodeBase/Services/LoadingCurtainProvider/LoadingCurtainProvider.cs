using CodeBase.Infrastructure;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.UI.Elements;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.LoadingCurtainProvider
{
  public class LoadingCurtainProvider : ILoadingCurtainProvider
  {
    private readonly IInstantiator _instantiator;
    private readonly IAssetProvider _assetProvider;
    private LoadingCurtain _loadingCurtain;

    public LoadingCurtainProvider(IInstantiator instantiator, IAssetProvider assetProvider)
    {
      _instantiator = instantiator;
      _assetProvider = assetProvider;
    }

    public async UniTask InitializeAsync()
    {
      GameObject prefab = await _assetProvider.Load<GameObject>(InfrastructureAssetPath.LoadingCurtainKey);
      _loadingCurtain = _instantiator.InstantiatePrefabForComponent<LoadingCurtain>(prefab);
    }

    public void Show() =>
      _loadingCurtain.Show();

    public void Hide() =>
      _loadingCurtain.Hide();
  }
}