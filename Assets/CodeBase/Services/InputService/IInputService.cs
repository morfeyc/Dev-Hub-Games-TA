using System;
using UnityEngine;

namespace CodeBase.Services.InputService
{
  public interface IInputService
  {
    Vector2 MoveDirection { get; }
    event Action<Vector2> OnFingerDown;
    event Action OnFingerUp;
  }
}