using CodeBase.Gameplay.Features.MemeQuiz.AnswerZone;
using UnityEngine;

namespace CodeBase.Gameplay.Features.Avatar
{
  public class AvatarCollisions : MonoBehaviour
  {
    [SerializeField] private Avatar _avatar;
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.TryGetComponent(out AnswerZone answerZone)) 
        _avatar.SetChosenAnswer(answerZone);
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.TryGetComponent(out AnswerZone _)) 
        _avatar.RemoveChosenAnswer();
    }
  }
}