using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Features.MemeQuiz.Meme;
using CodeBase.Services.StaticDataService;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace CodeBase.Gameplay.Features.MemeQuiz.Service
{
  public class MemeQuizService : IMemeQuizService, IInitializable
  {
    private readonly MemeDisplay _memeDisplay;
    private List<AnswerZone.AnswerZone> _answerZones;
    private readonly IStaticDataService _staticDataService;

    private MemesConfig _memesConfig;
    private List<string> _allAnswers;
    private int _prevMemeId = -1;

    public MemeQuizService(IStaticDataService staticDataService, MemeDisplay memeDisplay, List<AnswerZone.AnswerZone> answerZones)
    {
      _staticDataService = staticDataService;
      _memeDisplay = memeDisplay;
      _answerZones = answerZones;
    }

    public void Initialize()
    {
      _memesConfig = _staticDataService.GetSingleConfig<MemesConfig>();
      _allAnswers = _memesConfig.Memes.Keys.ToList();
    }

    public void GenerateNewQuiz()
    {
      ResetHighlightColor();
      int randomMemeId = Random.Range(0, _memesConfig.Memes.Count);
      if (randomMemeId == _prevMemeId)
      {
        if (Random.value > 0.5f)
          randomMemeId += 1;
        else
          randomMemeId -= 1;

        if (randomMemeId >= _memesConfig.Memes.Count) 
          randomMemeId = 0;
        else if(randomMemeId < 0)
          randomMemeId = _memesConfig.Memes.Count - 1;
      }

      _prevMemeId = randomMemeId;

      (string correctAnswer, Sprite sprite) = _memesConfig.Memes.ElementAt(randomMemeId);
      _memeDisplay.SetMeme(sprite);

      List<string> allAnswersCopy = _allAnswers.Select(item => (string)item.Clone()).ToList();
      allAnswersCopy.Remove(correctAnswer);
      allAnswersCopy.Remove(_memesConfig.Memes.ElementAt(_prevMemeId).Key);
      string incorrectAnswer = allAnswersCopy[Random.Range(0, allAnswersCopy.Count)];
      
      // we assume that we always have 2 answer zones
      _answerZones = _answerZones.OrderBy(_ => Random.value).ToList();
      _answerZones[0].SetAnswer(correctAnswer, true);
      _answerZones[1].SetAnswer(incorrectAnswer, false);
    }

    public AnswerZone.AnswerZone GetCorrectAnswerZone() => 
      _answerZones.Find(az => az.IsCorrectAnswer);
    
    public AnswerZone.AnswerZone GetIncorrectAnswerZone() => 
      _answerZones.Find(az => !az.IsCorrectAnswer);

    private void ResetHighlightColor()
    {
      foreach (AnswerZone.AnswerZone answerZone in _answerZones)
        answerZone.DecalColorSetter.ResetColor();
    }

    public void HighlightCorrectAnswer()
    {
      foreach (AnswerZone.AnswerZone answerZone in _answerZones) 
        answerZone.DecalColorSetter.SetColor(answerZone.IsCorrectAnswer ? Color.green : Color.red);
    }
  }
}