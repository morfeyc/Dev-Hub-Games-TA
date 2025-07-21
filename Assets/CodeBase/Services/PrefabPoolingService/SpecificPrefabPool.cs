using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Services.PrefabPoolingService
{
  public class SpecificPrefabPool
  {
    public GameObject PoolRoot { get; private set; }
    
    private readonly IInstantiator _instantiator;
    private readonly GameObject _prefab;
    private readonly Queue<GameObject> _pool;

    private int _prewarmed; 

    public SpecificPrefabPool(IInstantiator instantiator, GameObject prefab)
    {
      _instantiator = instantiator;
      _prefab = prefab;

      _pool = new Queue<GameObject>();
    }

    public void Initialize()
    {
      PoolRoot = new GameObject($"{_prefab.name}_pool_{Guid.NewGuid().ToString()}");
    }

    public void Prewarm(int count)
    {
      if(_prewarmed >= count)
        return;
      
      _prewarmed = count;
      for (int i = 0; i < count - _prewarmed; i++)
      {
        GameObject obj = _instantiator.InstantiatePrefab(_prefab);
        obj.transform.SetParent(PoolRoot.transform, false);
        obj.SetActive(false);
      }
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent)
    {
      if (!_pool.TryDequeue(out GameObject result))
        result = _instantiator.InstantiatePrefab(_prefab);

      result.transform.position = position;
      result.transform.rotation = rotation;
      result.transform.SetParent(parent, false);
      result.SetActive(true);
      return result;
    }

    public void Despawn(GameObject gameObject)
    {
      if(!gameObject || !PoolRoot)
        return;
      
      gameObject.SetActive(false);
      gameObject.transform.SetParent(PoolRoot.transform);
      _pool.Enqueue(gameObject);
    }
  }
}