using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.StaticDataService
{
  public interface IStaticDataService
  {
    UniTask LoadAll();
    TConfig GetSingleConfig<TConfig>() where TConfig : ScriptableObject;
    List<TConfig> GetMultipleConfigs<TConfig>() where TConfig : ScriptableObject;
  }
}