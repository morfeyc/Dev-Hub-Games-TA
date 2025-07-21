using System;
using System.Collections.Generic;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    private readonly Dictionary<string, List<AsyncOperationHandle>> _assetsHandles = new();
    
    private readonly DSender _dSender = new (name: "[Assets]");

    public async UniTask InitializeAsync() =>
      await Addressables.InitializeAsync().ToUniTask();

    public async UniTask<TAsset> Load<TAsset>(AssetReference assetReference) where TAsset : class
    {
      DLogger.Message(_dSender)
        .WithText($"Load by assetReference: {assetReference.AssetGUID.White()}")
        .Log();
      
      if (TryToGetCachedAsset(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
        return completedHandle.Result as TAsset;

      return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<TAsset>(assetReference), assetReference.AssetGUID);
    }

    public async UniTask<TAsset> Load<TAsset>(string address) where TAsset : class
    {
      DLogger.Message(_dSender)
        .WithText($"Load by address: {address.White()}")
        .Log();
      
      if (TryToGetCachedAsset(address, out AsyncOperationHandle completedHandle))
        return completedHandle.Result as TAsset;

      return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<TAsset>(address), address);
    }

    public async UniTask<TAsset[]> LoadAll<TAsset>(List<string> addresses) where TAsset : class
    {
      var tasks = new List<UniTask<TAsset>>(addresses.Count);
      
      DebugMessageBuilder dLogger = DLogger.Message(_dSender)
        .WithText("Loaded multiple assets:");

      foreach (string address in addresses)
      {
        tasks.Add(Load<TAsset>(address));
        dLogger.WithText($"\n{address}");
      }

      dLogger.Log();
      return await UniTask.WhenAll(tasks);
    }

    public async UniTask<TAsset[]> LoadAll<TAsset>(List<AssetReference> assetReferences) where TAsset : class
    {
      var tasks = new List<UniTask<TAsset>>(assetReferences.Count);

      DebugMessageBuilder dLogger = DLogger.Message(_dSender)
        .WithText("Loaded multiple assets:");

      foreach (AssetReference assetReference in assetReferences)
      {
        tasks.Add(Load<TAsset>(assetReference.AssetGUID));
        dLogger.WithText($"\n{assetReference.AssetGUID}");
      }
      
      dLogger.Log();
      return await UniTask.WhenAll(tasks);
    }

    public async UniTask WarmupAssetsByLabel(string label)
    {
      List<string> assetsList = await GetAssetsListByLabel(label);
      
      DLogger.Message(_dSender)
        .WithText($"Warmup by label: {label.White()}")
        .Log();
      
      await LoadAll<object>(assetsList);
    }

    public async UniTask ReleaseAssetsByLabel(string label)
    {
      List<string> assetsList = await GetAssetsListByLabel(label);

      DebugMessageBuilder dLogger = DLogger.Message(_dSender)
        .WithText($"Release assets by label: {label.White()}");

      foreach (string assetKey in assetsList)
      {
        dLogger.WithText($"\n{assetKey} release");
        if (_assetsHandles.TryGetValue(assetKey, out List<AsyncOperationHandle> handler))
        {
          foreach (AsyncOperationHandle operationHandle in handler)
            Addressables.Release(operationHandle);

          _assetsHandles.Remove(assetKey);
        }
      }
      
      dLogger.Log();
    }

    public async UniTask<List<string>> GetAssetsListByLabel<TAsset>(string label) =>
      await GetAssetsListByLabel(label, typeof(TAsset));

    public async UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null)
    {
      AsyncOperationHandle<IList<IResourceLocation>> operationHandle = Addressables.LoadResourceLocationsAsync(label, type);

      IList<IResourceLocation> locations = await operationHandle.ToUniTask();

      var assetKeys = new List<string>(locations.Count);

      foreach (IResourceLocation location in locations)
        assetKeys.Add(location.PrimaryKey);

      Addressables.Release(operationHandle);
      return assetKeys;
    }

    public void Cleanup()
    {
      DebugMessageBuilder dLogger = DLogger.Message(_dSender)
        .WithText($"Cleanup")
        .WithText("\nReleased:");
      
      foreach (List<AsyncOperationHandle> resourceHandles in _assetsHandles.Values)
      foreach (AsyncOperationHandle handle in resourceHandles)
      {
        Addressables.Release(handle);
        dLogger.WithText($"\nReleased: {handle.DebugName}");
      }

      dLogger.Log();
      _assetsHandles.Clear();
    }

    private bool TryToGetCachedAsset(string key, out AsyncOperationHandle completeHandle)
    {
      if (_assetsHandles.TryGetValue(key, out List<AsyncOperationHandle> handlesList))
      {
        foreach (AsyncOperationHandle handle in handlesList)
        {
          if (handle.Status == AsyncOperationStatus.Succeeded)
          {
            completeHandle = handle;
            return true;
          }
        }
      }

      completeHandle = default;
      return false;
    }

    private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
    {
      if (!_assetsHandles.TryGetValue(cacheKey, out List<AsyncOperationHandle> resourceHandle))
      {
        resourceHandle = new List<AsyncOperationHandle>();
        _assetsHandles[cacheKey] = resourceHandle;
      }

      resourceHandle.Add(handle);
      return await handle.ToUniTask();
    }
  }
}