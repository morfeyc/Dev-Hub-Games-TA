using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.StateTypes
{
  public interface IPayloadedState<TPayload> : IExitableState
  {
    UniTask Enter(TPayload payload);
  }
}