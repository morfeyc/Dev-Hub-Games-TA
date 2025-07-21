using UnityEditor;

namespace CodeBase.Editor
{
#if UNITY_EDITOR
  public class ToggleInspectorLock
  {
    static ToggleInspectorLock()
    {
#pragma warning disable CS8321
      [MenuItem("Tools/Toggle Inspector Lock %SPACE")]
      static void ToggleAction()
      {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
      }
#pragma warning restore CS8321
    }
  }
#endif
}