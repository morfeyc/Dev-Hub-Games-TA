using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Features.MemeQuiz.Meme
{
  public class MemeDisplay : MonoBehaviour
  {
    [SerializeField] private Image _bannerImage;

    public void SetMeme(Sprite memeSprite)
    {
      _bannerImage.sprite = memeSprite;
    }
    
    public void ClearMeme()
    {
      if (_bannerImage)
      {
        _bannerImage.sprite = null;
      }
    }
  }
}