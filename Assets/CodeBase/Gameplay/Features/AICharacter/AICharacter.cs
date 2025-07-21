using CodeBase.Gameplay.Features.MemeQuiz.AnswerZone;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.Features.AICharacter
{
  public class AICharacter : MonoBehaviour
  {
    [field: SerializeField] public NavMeshAgent NavMeshAgent {private set; get; }
    [SerializeField] private AIMovement _aiMovement;
    [SerializeField] private AIAnimator _aiAnimator;
    public AICharactersConfig AIConfig { private set; get; }
    
    private AnswerZone _assignedAnswerZone;
    private Vector3 _startPosition;

    public void Initialize(AICharactersConfig aiConfig, Vector3 startPos)
    {
      AIConfig = aiConfig;
      _startPosition = startPos;
      NavMeshAgent.speed = aiConfig.MoveSpeed;
    }
    
    public void ResetPosition()
    {
      RandomizeStartPosition();
      _aiMovement.RemoveDestination();
      _assignedAnswerZone = null;
    }

    private void RandomizeStartPosition()
    {
      transform.position = RandomPos(_startPosition, AIConfig.GroupSpreadOut);
    }
    
    public void Celebrate() => 
      _aiAnimator.PlayDance();

    public bool IsAssignedAnswerCorrectOne() => 
      _assignedAnswerZone && _assignedAnswerZone.IsCorrectAnswer;

    public void AssignAnswerZone(AnswerZone answerZone)
    {
      _assignedAnswerZone = answerZone;
      _aiMovement.SetDestination(RandomPos(_assignedAnswerZone.transform.position, AIConfig.MovingOffset));
    }

    private Vector3 RandomPos(Vector3 from, float offsetAmount)
    {
      Vector2 offset = Random.insideUnitCircle;
      Vector3 randomPos = from + offsetAmount * new Vector3(offset.x, 0, offset.y);
      return NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 5, NavMesh.AllAreas) 
        ? hit.position 
        : from;
    }
  }
}
