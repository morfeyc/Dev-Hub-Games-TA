using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.Features.AICharacter
{
  public class AICharactersConfig : ScriptableObject
  {
    [TabGroup("Asset")] public AssetReference AssetReference;
    [TabGroup("Settings")] public float MoveSpeed;
    [TabGroup("Settings")] public float GroupSpreadOut;
    [TabGroup("Settings")] public float MovingOffset;
  }
}