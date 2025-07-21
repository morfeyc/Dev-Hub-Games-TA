using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Gameplay.Features.MemeQuiz.Meme
{
  public class MemesConfig : SerializedScriptableObject
  {
    [DictionaryDrawerSettings(KeyLabel = "Meme Answer", ValueLabel = "Meme Sprite Asset", DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<string, Sprite> Memes = new();
  }
}