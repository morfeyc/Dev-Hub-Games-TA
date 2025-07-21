using CodeBase.Utils.SmartDebug;

namespace CodeBase.Infrastructure.StateMachine
{
  public class GameStateMachine : StateMachine
  {
    public GameStateMachine()
    {
      Logger = new StateMachineLogger(new DSender(name: "[Game State Machine]"));
    }
  }
}