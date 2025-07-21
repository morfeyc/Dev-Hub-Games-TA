using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.TimerWindow
{
  public class TimerDisplay : MonoBehaviour
  {
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _text;

    public void SetTime(int time, float fillAmount)
    {
      _text.text = time.ToString();
      _fillImage.fillAmount = fillAmount;
    }
  }
}