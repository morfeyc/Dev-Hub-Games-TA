using CodeBase.Utils.SmartDebug;

namespace CodeBase.Infrastructure.StateMachine
{
  public class SceneStateMachine : StateMachine
  {
    public SceneStateMachine(string sceneName)
    {
      Logger = new StateMachineLogger(new DSender(name: $"[Scene State Machine|{sceneName}]"));
    }
  }
}