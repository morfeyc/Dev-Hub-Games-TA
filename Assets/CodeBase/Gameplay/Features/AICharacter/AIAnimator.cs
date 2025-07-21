using UnityEngine;

namespace CodeBase.Gameplay.Features.AICharacter
{
  public class AIAnimator : MonoBehaviour
  {
    [SerializeField] private AICharacter _aiCharacter;
    [SerializeField] private Animator _animator;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Dance = Animator.StringToHash("Dance");

    private void Update()
    {
      float speed = Mathf.Clamp01(_aiCharacter.NavMeshAgent.velocity.magnitude / _aiCharacter.AIConfig.MoveSpeed);
      _animator.SetFloat(Speed, speed);
    }

    public void PlayDance() => 
      _animator.SetTrigger(Dance);
  }
}