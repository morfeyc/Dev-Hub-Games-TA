using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Features.AICharacter;
using CodeBase.Gameplay.Features.Avatar;
using CodeBase.Gameplay.Features.Joystick;
using CodeBase.Gameplay.Features.MemeQuiz.Meme;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.UI.Windows;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.StaticDataService
{
  public class StaticDataService : IStaticDataService
  {
    private readonly Dictionary<Type, object> _singleConfigs = new();
    private readonly Dictionary<Type, List<object>> _multiConfigs = new();
    private readonly IAssetProvider _assetProvider;
    private readonly DSender _dSender = new(name: "[StaticData]");

    public StaticDataService(IAssetProvider assetProvider)
    {
      _assetProvider = assetProvider;
    }

    public async UniTask LoadAll()
    {
      DLogger.Message(_dSender)
        .WithText("Start loading static data")
        .Log();
      
      var tasks = new List<UniTask>
      {
        LoadSingleConfig<WindowsConfig>(),
        LoadSingleConfig<JoystickConfig>(),
        LoadSingleConfig<AvatarConfig>(),
        LoadSingleConfig<MemesConfig>(),
        LoadSingleConfig<AICharactersConfig>(),
        LoadSingleConfig<GameplaySettings>()
      };

      await UniTask.WhenAll(tasks);

      DLogger.Message(_dSender)
        .WithText("Finish loading static data")
        .Log();
    }

    public TConfig GetSingleConfig<TConfig>() where TConfig : ScriptableObject
    {
      if (_singleConfigs.TryGetValue(typeof(TConfig), out var config))
      {
        return config as TConfig;
      }
      else
      {
        DLogger.Message(_dSender)
          .WithText($"Single config of type {typeof(TConfig)} not founded in static data")
          .WithFormat(DebugFormat.Exception)
          .Log();
        return null;
      }
    }

    public List<TConfig> GetMultipleConfigs<TConfig>() where TConfig : ScriptableObject
    {
      if (_multiConfigs.TryGetValue(typeof(TConfig), out List<object> configs))
      {
        return configs.Cast<TConfig>().ToList();
      }
      else
      {
        DLogger.Message(_dSender)
          .WithText($"Multiple configs of type {typeof(TConfig)} not founded in static data")
          .WithFormat(DebugFormat.Exception)
          .Log();
        return null;
      }
    }

    private async UniTask<TConfig[]> GetConfigs<TConfig>() where TConfig : class
    {
      List<string> keys = await GetConfigKeys<TConfig>();
      return await _assetProvider.LoadAll<TConfig>(keys);
    }

    private async UniTask<List<string>> GetConfigKeys<TConfig>()
    {
      return await _assetProvider.GetAssetsListByLabel<TConfig>(AssetLabels.Config);
    }

    private async UniTask<TConfig> LoadSingleConfig<TConfig>() where TConfig : class
    {
      TConfig[] configs = await GetConfigs<TConfig>();
      if (configs.Length > 0)
      {
        _singleConfigs.Add(typeof(TConfig), configs.First());
        DLogger.Message(_dSender)
          .WithText($"Single config of type {typeof(TConfig)} loaded")
          .Log();
      }
      else
      {
        DLogger.Message(_dSender)
          .WithText($"Single config of type {typeof(TConfig)} not founded in static data")
          .WithFormat(DebugFormat.Exception)
          .Log();
      }

      if (configs.Length > 1)
      {
        DLogger.Message(_dSender)
          .WithText($"Single config of type {typeof(TConfig)} has more than one instance")
          .WithFormat(DebugFormat.Warning)
          .Log();
      }

      return configs.First();
    }
    
    private async UniTask<TConfig[]> LoadMultipleConfigs<TConfig>() where TConfig : class
    {
      TConfig[] loadedConfigs = await GetConfigs<TConfig>();
      Type configType = typeof(TConfig);
      if (loadedConfigs.Length > 0)
      {
        if (!_multiConfigs.ContainsKey(configType))
          _multiConfigs[configType] = new List<object>();

        foreach (TConfig config in loadedConfigs)
          _multiConfigs[configType].Add(config);
      }
      else
      {
        DLogger.Message(_dSender)
          .WithText($"Multiple configs of type {typeof(TConfig)} not founded in static data")
          .WithFormat(DebugFormat.Exception)
          .Log();
      }

      return loadedConfigs;
    }
  }
}