using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.StateTypes
{
  public interface IState : IExitableState
  {
    UniTask Enter();
  }
}