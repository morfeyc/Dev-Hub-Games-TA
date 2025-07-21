using UnityEngine;

namespace CodeBase.Utils
{
  public class SetTargetFramerate : MonoBehaviour
  {
    [SerializeField] private int TargetFramerate;

    private void Awake()
    {
      Application.targetFrameRate = TargetFramerate;
    }
  }
}