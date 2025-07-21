using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class LookAtMainCamera : MonoBehaviour
  {
    private Camera _mainCamera;

    private void Awake()
    {
      _mainCamera = Camera.main;
    }

    private void Update()
    {
      transform.LookAt(_mainCamera.transform.position);
      transform.Rotate(0,180,0);
    }
  }
}