using System;
using CodeBase.Services.InputService;
using CodeBase.Services.StaticDataService;
using CodeBase.UI.Factory;
using CodeBase.Utils;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Gameplay.Features.Joystick.Service
{
  public class JoystickService : IJoystickService, IDisposable
  {
    private readonly IUIFactory _uiFactory;
    private readonly IStaticDataService _staticDataService;
    private readonly IInputService _inputService;

    private JoystickConfig _joystickConfig;
    private Joystick _joystickInstance;

    private readonly MinMax _horizontalBounce = new MinMax(0,0);
    private readonly MinMax _verticalBounce = new MinMax(0,0);
    private Vector2 _lastKnownPosition;
    private bool _isEnabled;

    public JoystickService(IUIFactory uiFactory, IStaticDataService staticDataService, IInputService inputService)
    {
      _staticDataService = staticDataService;
      _uiFactory = uiFactory;
      _inputService = inputService;
    }

    public async UniTask InitializeJoystick()
    {
      if (_joystickInstance)
      {
        DLogger.Message(DSenders.Empty)
          .WithText("Joystick already initialized.")
          .WithFormat(DebugFormat.Warning)
          .Log();
        return;
      }
      
      _joystickConfig = _staticDataService.GetSingleConfig<JoystickConfig>();

      DLogger.Message(DSenders.Empty)
        .WithText("Initializing Joystick UI...")
        .Log();
      
      _joystickInstance = await _uiFactory.CreateAndPlaceUIElement<Joystick>(_joystickConfig.AssetReference);
      
      _joystickInstance.CanvasGroup.alpha = 0f;
      _joystickInstance.CanvasGroup.interactable = false;
      _joystickInstance.CanvasGroup.blocksRaycasts = false;

      CalculateAllowedBounce();
      RegisterJoystick();
      
      DLogger.Message(DSenders.Empty)
        .WithText("Joystick UI created and prepared.")
        .Log();
    }

    public void EnableJoystick()
    {
      _isEnabled = true;
    }

    public void DisableJoystick()
    {
      _isEnabled = false;
      HideJoystick();
    }

    public void ShowJoystick(Vector2 screenPosition)
    {
      if(!_isEnabled)
        return;
      
      if (!IsPositionWithinBounds(screenPosition))
      {
        DLogger.Message(DSenders.Empty)
          .WithText($"Touch position {screenPosition} is outside allowed joystick bounds. Not showing joystick.")
          .Log();
        return;
      }

      _lastKnownPosition = screenPosition;
      
      SetJoystickPosition(screenPosition);
      _joystickInstance.CanvasGroup.alpha = 1f;
      _joystickInstance.CanvasGroup.interactable = true;
      _joystickInstance.CanvasGroup.blocksRaycasts = true;
      
      DLogger.Message(DSenders.Empty)
        .WithText("Joystick UI shown and positioned.")
        .Log();
    }

    public void HideJoystick()
    {
      _joystickInstance.CanvasGroup.alpha = 0f;
      _joystickInstance.CanvasGroup.interactable = false;
      _joystickInstance.CanvasGroup.blocksRaycasts = false;
      DLogger.Message(DSenders.Empty)
        .WithText("Joystick UI hidden.")
        .Log();
    }

    public void Cleanup()
    {
      if (!_joystickInstance) 
        return;
      
      Object.Destroy(_joystickInstance);
      _joystickInstance = null;
      
      DLogger.Message(DSenders.Empty)
        .WithText("Joystick UI cleaned up.")
        .Log();
    }

    private void SetJoystickPosition(Vector2 screenPosition)
    {
        Camera uiCamera = _joystickInstance.GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay
          ? null 
          : Camera.main;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _joystickInstance.UIRootRectTransform,
            screenPosition,
            uiCamera,
            out Vector2 localPoint);
        
        _joystickInstance.JoystickRectTransform.anchoredPosition = localPoint;
        DLogger.Message(DSenders.Empty)
          .WithText($"Joystick positioned at screen: {screenPosition}, local: {localPoint}")
          .Log();
    }

    private void CalculateAllowedBounce()
    {
      float screenWidth = Screen.width;
      float screenHeight = Screen.height;
      
      _horizontalBounce.Min = screenWidth * _joystickConfig.HorizontalBorderPercentage;
      _horizontalBounce.Max = screenWidth * (1 - _joystickConfig.HorizontalBorderPercentage);
      _verticalBounce.Min = screenHeight * _joystickConfig.VerticalBorderPercentage;
      _verticalBounce.Max = screenHeight * (1 - _joystickConfig.VerticalBorderPercentage);
    }
    
    private bool IsPositionWithinBounds(Vector2 screenPosition)
    {
      return screenPosition.x >= _horizontalBounce.Min && screenPosition.x <= _horizontalBounce.Max &&
             screenPosition.y >= _verticalBounce.Min && screenPosition.y <= _verticalBounce.Max;
    }
    
    private void RegisterJoystick()
    {
      _inputService.OnFingerDown += HandleFingerDown;
      _inputService.OnFingerUp += HandleFingerUp;
      HideJoystick();
    }

    private void ReleaseJoystickCallbacks()
    {
      _inputService.OnFingerDown -= HandleFingerDown;
      _inputService.OnFingerUp -= HandleFingerUp;
    }

    private void HandleFingerDown(Vector2 screenPosition) => 
      ShowJoystick(screenPosition);

    private void HandleFingerUp() => 
      HideJoystick();

    public void Dispose()
    {
      Cleanup();
      ReleaseJoystickCallbacks();
    }
  }
}