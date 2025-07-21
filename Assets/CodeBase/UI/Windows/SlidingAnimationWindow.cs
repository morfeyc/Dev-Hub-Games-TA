using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Windows
{
  public class SlidingAnimationWindow : WindowBase
  {
    [SerializeField] private Transform _transformToAnimate;
    [SerializeField] private Transform _openingAnimationStartPoint;
    [SerializeField] private float _animationDuration;

    private Vector3 _startWindowPosition;

    protected override void OnAwake()
    {
      base.OnAwake();
      _startWindowPosition = _transformToAnimate.position;
    }

    public override void Open()
    {
      _transformToAnimate.position = _openingAnimationStartPoint.position;
      _transformToAnimate.gameObject.SetActive(true);

      _transformToAnimate
        .DOMoveY(_startWindowPosition.y, _animationDuration)
        .SetEase(Ease.Linear)
        .SetLink(this.gameObject);
    }

    public override void Close()
    {
      _transformToAnimate
        .DOMoveY(_openingAnimationStartPoint.position.y, _animationDuration)
        .SetEase(Ease.Linear)
        .SetLink(this.gameObject);

      _transformToAnimate.gameObject.SetActive(false);
    }
  }
}