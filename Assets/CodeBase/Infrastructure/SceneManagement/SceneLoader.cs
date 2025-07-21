using System;
using System.Diagnostics;
using System.Globalization;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.SceneManagement
{
  public class SceneLoader : ISceneLoader
  {
    private readonly DSender _sceneLoader = new(name: "[SceneLoader]");
    private readonly Stopwatch _stopwatch = new();

    public async UniTask Load(string sceneName, Action onLoaded = null)
    {
      DLogger.Message(_sceneLoader)
        .WithText($"Start loading scene \"{sceneName.White()}\"")
        .Log();

      _stopwatch.Reset();
      _stopwatch.Start();
      
      AsyncOperationHandle<SceneInstance> handler = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);
      await handler.ToUniTask();
      await handler.Result.ActivateAsync().ToUniTask();
      while (!handler.IsDone)
        await UniTask.Yield();
      
      _stopwatch.Stop();
      
      DLogger.Message(_sceneLoader)
        .WithText($"Loaded scene \"{sceneName.White()}\" in {_stopwatch.Elapsed.TotalMilliseconds.ToString(CultureInfo.CurrentCulture)}ms")
        .Log();
    }
  }
}