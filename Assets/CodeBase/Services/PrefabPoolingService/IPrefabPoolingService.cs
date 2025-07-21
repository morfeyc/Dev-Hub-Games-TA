using UnityEngine;

namespace CodeBase.Services.PrefabPoolingService
{
  public interface IPrefabPoolingService
  {
    void Prewarm(GameObject prefab, int count);
    GameObject Spawn(GameObject gameObject, Transform parent = null);
    GameObject Spawn(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent = null);
    TComponent Spawn<TComponent>(GameObject gameObject, Transform parent) where TComponent : MonoBehaviour;
    TComponent Spawn<TComponent>(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent) where TComponent : MonoBehaviour;
    void Despawn(GameObject gameObject);
    void Despawn<TComponent>(TComponent component) where TComponent : MonoBehaviour;
  }
}