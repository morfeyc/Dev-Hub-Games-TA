using UnityEngine;

namespace CodeBase.Utils
{
  public class SetMainCameraToCanvas : MonoBehaviour
  {
    private void Awake()
    {
      if(TryGetComponent(out Canvas canvas))
        canvas.worldCamera = Camera.main;
    }
  }
}