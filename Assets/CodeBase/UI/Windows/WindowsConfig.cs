using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnassignedField.Global

namespace CodeBase.UI.Windows
{
  public class WindowsConfig : SerializedScriptableObject
  {
    [DictionaryDrawerSettings(KeyLabel = "Window Id", ValueLabel = "Window Asset", DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<WindowId, AssetReference> WindowConfigs;
  }
}