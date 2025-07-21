using CodeBase.Gameplay.Features.AICharacter.Provider;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PrefabPoolingService;
using CodeBase.Services.StaticDataService;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Features.AICharacter.Factory
{
  public class AICharacterFactory : IAICharacterFactory
  {
    private readonly IPrefabPoolingService _poolingService;
    private readonly IAssetProvider _assetProvider;
    private readonly IStaticDataService _staticDataService;
    private readonly IAICharactersProvider _aiCharactersProvider;

    private AICharactersConfig _aiConfig;
    private GameObject _aiPrefab;

    public AICharacterFactory(IPrefabPoolingService poolingService, 
      IAssetProvider assetProvider, 
      IStaticDataService staticDataService,
      IAICharactersProvider aiCharactersProvider)
    {
      _aiCharactersProvider = aiCharactersProvider;
      _poolingService = poolingService;
      _assetProvider = assetProvider;
      _staticDataService = staticDataService;
    }

    public async UniTask InitializeAsync()
    {
      _aiConfig = _staticDataService.GetSingleConfig<AICharactersConfig>();
      _aiPrefab = await _assetProvider.Load<GameObject>(_aiConfig.AssetReference);
      _poolingService.Prewarm(_aiPrefab, 5);
    }

    public AICharacter CreateAICharacter(Vector3 at)
    {
      if(!NavMesh.SamplePosition(at, out NavMeshHit hit, 5, NavMesh.AllAreas))
        DLogger.Message(DSenders.Empty)
          .WithText("Unable to spawn AI character. Invalid position")
          .WithFormat(DebugFormat.Error)
          .Log();
      
      AICharacter aiCharacter = _poolingService.Spawn<AICharacter>(_aiPrefab, hit.position, Quaternion.identity, null);
      aiCharacter.Initialize(_aiConfig, hit.position);
      _aiCharactersProvider.AddAICharacter(aiCharacter);
      return aiCharacter;
    }

    public void DespawnAICharacter(AICharacter aiCharacter)
    {
      _poolingService.Despawn(aiCharacter);
      _aiCharactersProvider.RemoveAICharacter(aiCharacter);
    }

    public void Cleanup()
    {
      _aiConfig = null;
      _aiPrefab = null;
    }
  }
}