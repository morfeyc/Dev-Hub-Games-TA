using CodeBase.Services.InputService;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Gameplay.Features.Avatar
{
  public class AvatarMovement : MonoBehaviour
  {
    [SerializeField] private Avatar _avatar;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private const float MinimalMoveInput = 0.001f;
    
    private Camera _mainCamera;
    private IInputService _inputService;
    private bool _isEnabled;

    [Inject]
    public void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }

    private void Awake()
    {
      _mainCamera = Camera.main;
    }

    private void Update()
    {
      if(_inputService.MoveDirection.sqrMagnitude < MinimalMoveInput || !_isEnabled)
        return;
      
      Vector3 cameraForward = _mainCamera.transform.forward;
      cameraForward.y = 0;
      cameraForward.Normalize(); 
      
      Vector3 cameraRight = _mainCamera.transform.right;
      cameraRight.y = 0;
      cameraRight.Normalize();
      
      Vector3 worldMoveDirection = cameraForward * _inputService.MoveDirection.y + cameraRight * _inputService.MoveDirection.x;
      
      Vector3 scaledMovement = worldMoveDirection * (_avatar.AvatarConfig.MoveSpeed * Time.deltaTime);
      _navMeshAgent.Move(scaledMovement);

      if (worldMoveDirection != Vector3.zero) 
        transform.LookAt(transform.position + worldMoveDirection, Vector3.up);
    }

    public void DisableMovement()
    {
      _isEnabled = false;
    }

    public void EnableMovement()
    {
      _isEnabled = true;
    }
  }
}