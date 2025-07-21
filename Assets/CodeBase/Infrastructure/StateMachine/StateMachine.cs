using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.StateMachine.StateTypes;
using CodeBase.Utils.SmartDebug;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Infrastructure.StateMachine
{
  public abstract class StateMachine : IStateMachine, ITickable
  {
    public Type CurrentStateType { get; private set; }
    private readonly Dictionary<Type, IExitableState> _registeredStates = new();
    private IExitableState _activeState;
    private ITickableState _tickableState;

    protected StateMachineLogger Logger { get; set; } = new(new DSender("[Default State Machine]"));

    public async UniTask Enter<TState>() where TState : class, IState
    {
      IExitableState oldState = _activeState;
      IState state = await ChangeState<TState>();
      Logger.LogEnter(state, oldState);
      await state.Enter();
    }

    public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
      IExitableState oldState = _activeState;
      TState state = await ChangeState<TState>();
      Logger.LogEnter(payload, state, oldState);
      await state.Enter(payload);
    }

    public void RegisterState<TState>(TState state) where TState : IExitableState =>
      _registeredStates.Add(typeof(TState), state);

    public void Tick()
    {
      _tickableState?.Tick();
    }

    private async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
    {
      if (_activeState != null)
        await _activeState.Exit();

      TState state = GetState<TState>();
      CurrentStateType = typeof(TState);
      _activeState = state;

      _tickableState = null;
      if (_activeState is ITickableState tickableState)
        _tickableState = tickableState;

      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState =>
      _registeredStates[typeof(TState)] as TState;
  }
}