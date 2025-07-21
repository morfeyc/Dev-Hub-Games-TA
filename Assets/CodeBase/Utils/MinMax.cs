using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Utils
{
  [Serializable]
  [InlineProperty]
  public class MinMax
  {
    [HorizontalGroup("Split", 0.5f)]
    [LabelWidth(30)]
    [PropertyOrder(1)]
    public float Min;
    [HorizontalGroup("Split", 0.5f)]
    [LabelWidth(30)]
    [PropertyOrder(2)]
    public float Max;

    public MinMax(float min, float max)
    {
      Min = min;
      Max = max;
    }

    public float Random() => 
      UnityEngine.Random.Range(Min, Max);
    
    public float Interpolate01(float value)
    {
      return value switch
      {
        < 0 => Min,
        > 1 => Max,
        _ => Min + Mathf.Clamp01(value) * (Max - Min)
      };
    }
  }
  
  [Serializable]
  [InlineProperty]
  public class MinMaxInt
  {
    [HorizontalGroup("Split", 0.5f)]
    [LabelWidth(30)]
    [PropertyOrder(1)]
    public int Min;
    [HorizontalGroup("Split", 0.5f)]
    [LabelWidth(30)]
    [PropertyOrder(2)]
    public int Max;

    public int Random() => 
      UnityEngine.Random.Range(Min, Max);

    public int Interpolate01(float value)
    {
      return value switch
      {
        < 0 => Min,
        > 1 => Max,
        _ => (int)Math.Round(Min + Mathf.Clamp01(value) * (Max - Min))
      };
    }
  }
}