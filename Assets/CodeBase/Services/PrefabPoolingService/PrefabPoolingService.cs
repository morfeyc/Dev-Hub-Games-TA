using System.Collections.Generic;
using CodeBase.Utils.SmartDebug;
using UnityEngine;
using Zenject;
//TODO: add pool 'preloading'
namespace CodeBase.Services.PrefabPoolingService
{
  public class PrefabPoolingService : IPrefabPoolingService
  {
    private readonly IInstantiator _instantiator;

    private readonly Dictionary<int, SpecificPrefabPool> _prefabsToPoolsMap;
    private readonly Dictionary<int, SpecificPrefabPool> _objectToPoolMap;
    
    private readonly DSender _dSender = new ("[PrefabPoolingService]");

    public PrefabPoolingService(IInstantiator instantiator)
    {
      _instantiator = instantiator;

      _prefabsToPoolsMap = new();
      _objectToPoolMap = new();
    }
    
    public void Prewarm(GameObject prefab, int count)
    {
      if (!_prefabsToPoolsMap.TryGetValue(prefab.GetInstanceID(), out SpecificPrefabPool pool))
        pool = CreateNewPool(prefab);
    
      pool.Prewarm(count);
    }

    public GameObject Spawn(GameObject gameObject, Transform parent = null) => 
      Spawn(gameObject, Vector3.zero, Quaternion.identity, parent);

    public GameObject Spawn(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent = null)
    {
      if (!_prefabsToPoolsMap.TryGetValue(gameObject.GetInstanceID(), out SpecificPrefabPool pool))
        pool = CreateNewPool(gameObject);

      GameObject spawnedObject = pool.Spawn(position, rotation, parent);
      _objectToPoolMap.Add(spawnedObject.GetInstanceID(), pool);
      
      DLogger.Message(_dSender)
        .WithText($"Spawned: {spawnedObject.name}")
        .Log();
      
      return spawnedObject;
    }
    
    public TComponent Spawn<TComponent>(GameObject gameObject, Transform parent) where TComponent : MonoBehaviour
    {
      return Spawn<TComponent>(gameObject, Vector3.zero, Quaternion.identity, parent);
    }

    public TComponent Spawn<TComponent>(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent) where TComponent : MonoBehaviour
    {
      GameObject spawnedObject = Spawn(gameObject, position, rotation, parent);
      if (spawnedObject.TryGetComponent(out TComponent component))
        return component;
      
      DLogger.Message(_dSender)
        .WithText($"{spawnedObject.name} missing component {typeof(TComponent).Name}")
        .WithFormat(DebugFormat.Exception)
        .Log();
      
      return null;
    }

    private SpecificPrefabPool CreateNewPool(GameObject gameObject)
    {
      SpecificPrefabPool pool = _instantiator.Instantiate<SpecificPrefabPool>(new[] { gameObject });
      pool.Initialize();
      _prefabsToPoolsMap.Add(gameObject.GetInstanceID(), pool);
      
      DLogger.Message(_dSender)
        .WithText($"Created new pool: {pool.PoolRoot.name}")
        .Log();
      return pool;
    }
    
    public void Despawn(GameObject gameObject)
    {
      int instanceID = gameObject.GetInstanceID();
      if(_objectToPoolMap.TryGetValue(instanceID, out SpecificPrefabPool pool))
      {
        _objectToPoolMap.Remove(instanceID);
        pool.Despawn(gameObject);
        
        DLogger.Message(_dSender)
          .WithText($"Despawned: {gameObject.name}")
          .Log();
      }
    }
    
    public void Despawn<TComponent>(TComponent component) where TComponent : MonoBehaviour => 
      Despawn(component.gameObject);
  }
}