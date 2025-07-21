using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CodeBase.Gameplay.Features.MemeQuiz.AnswerZone
{
  public class DecalColorSetter : MonoBehaviour
  {
    [SerializeField] private DecalProjector _decalProjector;
    [SerializeField] private Color _startColor;
    
    private static readonly int Tint = Shader.PropertyToID("_Tint");

    private void Start()
    {
      _decalProjector.material = new Material(_decalProjector.material);
      SetColor(_startColor);
    }

    public void SetColor(Color color)
    {
      _decalProjector.material.SetColor(Tint, color);
    }

    [ContextMenu("ResetColor")]
    public void ResetColor()
    {
      SetColor(_startColor);
    }
  }
}
