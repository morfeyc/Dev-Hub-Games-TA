using System.Collections.Generic;
using CodeBase.Gameplay.Features.MemeQuiz.Meme;
using CodeBase.Gameplay.Features.MemeQuiz.Service;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Features.MemeQuiz
{
  public class MemeQuizInstaller : MonoInstaller
  {
    [SerializeField] private MemeDisplay _memeDisplay;
    [SerializeField] private List<AnswerZone.AnswerZone> _answerZones;
    
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<MemeQuizService>().AsSingle().WithArguments(_memeDisplay, _answerZones);
    }
  }
}
