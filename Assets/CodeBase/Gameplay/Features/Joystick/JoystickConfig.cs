using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.Features.Joystick
{
  public class JoystickConfig : ScriptableObject
  {
    public AssetReference AssetReference;
    
    [Range(0, 0.49f)]
    [Tooltip("Percentage of screen width to reserve from left/right edges (e.g., 0.15 for 15%)")]
    public float HorizontalBorderPercentage = 0.15f;
    
    [Range(0, 0.49f)]
    [Tooltip("Percentage of screen height to reserve from top/bottom edges (e.g., 0.10 for 10%)")]
    public float VerticalBorderPercentage = 0.15f;
  }
}