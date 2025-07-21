using System;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticDataService;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Features.Avatar.Factory
{
  public class AvatarFactory : IAvatarFactory
  {
    private readonly IInstantiator _instantiator;
    private readonly IAssetProvider _assetProvider;
    private readonly IStaticDataService _staticDataService;

    public AvatarFactory(IInstantiator instantiator, IAssetProvider assetProvider, IStaticDataService staticDataService)
    {
      _instantiator = instantiator;
      _assetProvider = assetProvider;
      _staticDataService = staticDataService;
    }

    public async UniTask<Avatar> CreateAvatar(Vector3 position, Quaternion rotation)
    {
      AvatarConfig avatarConfig = _staticDataService.GetSingleConfig<AvatarConfig>();
      GameObject playerPrefab = await _assetProvider.Load<GameObject>(avatarConfig.AssetReference);
      if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
      {
        DLogger.Message(DSenders.Empty)
          .WithText("Avatar could not be created! Spawn position invalid")
          .WithFormat(DebugFormat.Error)
          .Log();
        throw new Exception("Avatar spawn position invalid");
      }
      
      Avatar avatar = _instantiator.InstantiatePrefabForComponent<Avatar>(playerPrefab, hit.position, rotation, null);
      avatar.Initialize(avatarConfig);
      return avatar;
    }
  }
}