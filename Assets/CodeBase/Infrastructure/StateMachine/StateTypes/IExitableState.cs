using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.StateTypes
{
  public interface IExitableState
  {
    UniTask Exit();
  }
}