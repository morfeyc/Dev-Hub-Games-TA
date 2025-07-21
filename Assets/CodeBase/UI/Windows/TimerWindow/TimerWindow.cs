using UnityEngine;

namespace CodeBase.UI.Windows.TimerWindow
{
  public class TimerWindow : SlidingAnimationWindow
  {
    [field: SerializeField] public TimerDisplay TimerDisplay { private set; get; }
  }
}