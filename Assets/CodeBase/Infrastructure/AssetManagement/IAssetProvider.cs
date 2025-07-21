using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
  public interface IAssetProvider
  {
    UniTask InitializeAsync();
    UniTask<TAsset> Load<TAsset>(AssetReference assetReference) where TAsset : class;
    UniTask<TAsset> Load<TAsset>(string address) where TAsset : class;
    UniTask<TAsset[]> LoadAll<TAsset>(List<string> addresses) where TAsset : class;
    UniTask<TAsset[]> LoadAll<TAsset>(List<AssetReference> assetReferences) where TAsset : class;
    UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label);
    UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null);
    UniTask WarmupAssetsByLabel(string label);
    UniTask ReleaseAssetsByLabel(string label);
    void Cleanup();
  }
}