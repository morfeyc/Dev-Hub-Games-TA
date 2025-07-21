using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.Features.Joystick.Service
{
  public interface IJoystickService
  {
    UniTask InitializeJoystick();
    void EnableJoystick();
    void DisableJoystick();
  }
}