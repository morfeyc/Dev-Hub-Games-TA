using System;
using CodeBase.Gameplay.Features.MemeQuiz.AnswerZone;
using UnityEngine;

namespace CodeBase.Gameplay.Features.Avatar
{
  public class Avatar : MonoBehaviour
  {
    [SerializeField] private AvatarAnimator _avatarAnimator;
    [SerializeField] private AvatarMovement _avatarMovement;
    public AvatarConfig AvatarConfig {private set; get;}
    private AnswerZone _chosenAnswerZone;
    
    private Vector3 _startPosition;

    private void Awake()
    {
      _startPosition = transform.position;
    }

    public void Initialize(AvatarConfig avatarConfig)
    {
      AvatarConfig = avatarConfig;
    }

    public void ResetPosition()
    {
      transform.position = _startPosition;
      RemoveChosenAnswer();
    }

    public void EnableMovement()
    {
      _avatarMovement.EnableMovement();
      _avatarAnimator.EnableMovement();
    }

    public void DisableMovement()
    {
      _avatarMovement.DisableMovement();
      _avatarAnimator.DisableMovement();
    }

    public void CelebrateRound() => 
      _avatarAnimator.PlayDance();
    
    public void CelebrateVictory() => 
      _avatarAnimator.PlayWin();

    public bool ChooseCorrectAnswer() => 
      _chosenAnswerZone && _chosenAnswerZone.IsCorrectAnswer;

    public void SetChosenAnswer(AnswerZone answerZone) => 
      _chosenAnswerZone = answerZone;

    public void RemoveChosenAnswer() => 
      _chosenAnswerZone = null;
  }
}