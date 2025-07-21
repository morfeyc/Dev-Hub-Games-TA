using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.Features.Avatar
{
  public class AvatarConfig : ScriptableObject
  {
    [TabGroup("Asset")] public AssetReference AssetReference;
    [TabGroup("Settings")] public float MoveSpeed;
  }
}