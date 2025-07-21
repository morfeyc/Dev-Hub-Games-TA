using System;
using CodeBase.Input;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace CodeBase.Services.InputService
{
  public class InputService : IInputService, IInitializable, IDisposable, PlayerInputActions.IPlayerActions
  {
    public Vector2 MoveDirection { get; private set; }
    public event Action<Vector2> OnFingerDown;
    public event Action OnFingerUp;
    
    private PlayerInputActions _inputActions;
    private Finger _currentMovementFinger;

    public void Initialize()
    {
      EnhancedTouchSupport.Enable();
      Touch.onFingerDown += HandleFingerDown;
      Touch.onFingerUp += HandleFingerUp;
      
      _inputActions = new PlayerInputActions();
      _inputActions.Player.SetCallbacks(this);
      _inputActions.Player.Enable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
      MoveDirection = context.ReadValue<Vector2>();
    }
    
    private void HandleFingerDown(Finger touchedFinger)
    {
      if (_currentMovementFinger != null) 
        return;
      
      _currentMovementFinger = touchedFinger;
      OnFingerDown?.Invoke(touchedFinger.screenPosition);
    }

    private void HandleFingerUp(Finger touchedFinger)
    {
      if (_currentMovementFinger != touchedFinger) 
        return;
      
      _currentMovementFinger = null;
      OnFingerUp?.Invoke();
      MoveDirection = Vector2.zero;
    }

    public void Dispose()
    {
      Touch.onFingerDown -= HandleFingerDown;
      Touch.onFingerUp -= HandleFingerUp;
      
      _inputActions.Player.Disable();
      _inputActions.Dispose();
      
      EnhancedTouchSupport.Disable();
    }
  }
}