using UnityEngine;

namespace CodeBase.Gameplay.Features.Joystick
{
  public class Joystick : MonoBehaviour
  {
    [field: SerializeField] public Canvas Canvas { get; private set; }
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
    [field: SerializeField] public RectTransform JoystickRectTransform { get; private set; }
    [field: SerializeField] public RectTransform UIRootRectTransform { get; private set; }
  }
}