namespace CodeBase.Gameplay.Features.MemeQuiz.Service
{
  public interface IMemeQuizService
  {
    void GenerateNewQuiz();
    void HighlightCorrectAnswer();
    AnswerZone.AnswerZone GetCorrectAnswerZone();
    AnswerZone.AnswerZone GetIncorrectAnswerZone();
  }
}