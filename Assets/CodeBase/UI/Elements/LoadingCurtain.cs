using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class LoadingCurtain : MonoBehaviour
  {
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeInTime = 0.5f;
    
    private bool _fadeInActive;
    private Tweener _tweener;

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }
    

    public void Show()
    {
      StopFadeInIfActive();
      gameObject.SetActive(true);
      _canvasGroup.alpha = 1;
    }

    public void Hide()
    {
      DoFadeIn();
    }

    private void DoFadeIn()
    {
      if(_fadeInActive)
        return;
      
      _fadeInActive = true;
      _tweener = DOVirtual.Float(_canvasGroup.alpha, 0, _fadeInTime, v => { _canvasGroup.alpha = v; })
        .OnComplete(() =>
        {
          _fadeInActive = false;
          gameObject.SetActive(false);
        });
    }

    private void StopFadeInIfActive()
    {
      if (_fadeInActive)
        _fadeInActive = false;
      
      _tweener.Kill();
    }
  }
}