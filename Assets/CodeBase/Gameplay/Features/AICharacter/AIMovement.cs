using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Gameplay.Features.AICharacter
{
  public class AIMovement : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    public void SetDestination(Vector3 targetPosition)
    {
      if (_navMeshAgent.isOnNavMesh && _navMeshAgent.isActiveAndEnabled)
      {
        _navMeshAgent.SetDestination(targetPosition);
      }
    }

    public void RemoveDestination()
    {
      _navMeshAgent.SetDestination(transform.position);
    }
  }
}