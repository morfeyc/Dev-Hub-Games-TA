using TMPro;
using UnityEngine;

namespace CodeBase.Gameplay.Features.MemeQuiz.AnswerZone
{
  public class AnswerZone : MonoBehaviour
  {
    [field: SerializeField] public DecalColorSetter DecalColorSetter { get; private set; }
    [SerializeField] private TextMeshProUGUI _answerText;
    public bool IsCorrectAnswer { get; private set; }

    public void SetAnswer(string answer, bool isCorrect)
    {
      _answerText.text = answer;
      IsCorrectAnswer = isCorrect;
    }
  }
}