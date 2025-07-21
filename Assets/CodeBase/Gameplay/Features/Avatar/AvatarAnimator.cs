using CodeBase.Services.InputService;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Features.Avatar
{
  public class AvatarAnimator : MonoBehaviour
  {
    [SerializeField] private Animator _animator;

    private IInputService _inputService;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Dance = Animator.StringToHash("Dance");
    private static readonly int Win = Animator.StringToHash("Win");
    
    private bool _isEnabled;

    [Inject]
    public void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }

    private void Update()
    {
      if(!_isEnabled)
        return;
      
      float speed = Mathf.Clamp01(_inputService.MoveDirection.magnitude);
      _animator.SetFloat(Speed, speed);
    }

    public void PlayDance()
    {
      _animator.SetTrigger(Dance);
    }

    public void PlayWin()
    {
      _animator.applyRootMotion = true;
      _animator.SetTrigger(Win);
    }

    public void EnableMovement()
    {
      _isEnabled = true;
    }

    public void DisableMovement()
    {
      _isEnabled = false;
      _animator.SetFloat(Speed, 0);
    }
  }
}