using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected List<Transition> Transitions;
    [Range(0, 5)][SerializeField] protected float TimeWaitHandle;

    private Coroutine _jobHandle;
    private WaitForSeconds _waitForSeconds;

    public StatusState Status { get; private set; }
    protected AIEnemyController AIController { get; private set; }

    public void Initialize(AIEnemyController aiController)
    {
        if (Status != StatusState.None)
            throw new InvalidOperationException($"—осто€ние \"{GetType().Name}\" уже инициализировано!");

        _waitForSeconds = new WaitForSeconds(TimeWaitHandle);
        RunActionBeforeInitialize(aiController);
        AIController = aiController;

        foreach (var transition in Transitions)
            transition.Initialize(aiController, this);

        UpdateStatus(StatusState.Initialized);
        RunActionAfterInitialize(aiController);
    }

    public void Enter()
    {
        if (Status != StatusState.Initialized && Status != StatusState.Exited)
            throw new InvalidOperationException($"Ќевозможно изменить статус состо€ни€ \"{GetType().Name}\" на \"{StatusState.Entered}\"!");

        RunActionBeforeEnter();

        foreach (var transition in Transitions)
            transition.Activate();

        UpdateStatus(StatusState.Entered);
        RunActionAfterEnter();
        StartHandle();
    }

    public void Exit()
    {
        if ((int)Status < (int)StatusState.Entered)
            throw new InvalidOperationException($"Ќевозможно изменить статус состо€ни€ \"{GetType().Name}\" на \"{StatusState.Exited}\"!");

        RunActionBeforeExit();

        foreach (var transition in Transitions)
            transition.Deactivate();

        StopHandle();
        UpdateStatus(StatusState.Exited);
        RunActionAfterExit();
    }

    public bool TryGetNextState(out State state)
    {
        state = null;
        Transition transition = Transitions.FirstOrDefault(transition => transition.NeedTransit);

        if (transition == null)
            return false;

        state = transition.TargetState;
        return true;
    }

    protected abstract void Work();

    protected virtual void RunActionBeforeInitialize(AIEnemyController aiController) { }

    protected virtual void RunActionAfterInitialize(AIEnemyController aiController) { }

    protected virtual void RunActionBeforeEnter() { }

    protected virtual void RunActionAfterEnter() { }

    protected virtual void RunActionBeforeExit() { }

    protected virtual void RunActionAfterExit() { }

    protected virtual bool CanHandle() => true;

    private void StartHandle()
    {
        if (_jobHandle != null)
            throw new InvalidOperationException($"ќбработка состо€ни€ \"{GetType().Name}\" уже запущена!");

        if (Status != StatusState.Entered)
            throw new InvalidOperationException($"Ќевозможно запустить обработку состо€ни€ \"{GetType().Name}\"! —татус состо€ни€ должен иметь значение - \"{StatusState.Entered}\"!");

        _jobHandle = StartCoroutine(Handle());
    }

    private void StopHandle()
    {
        if (_jobHandle == null)
            return;

        StopCoroutine(_jobHandle);
        CompleteHandle();
    }

    private void CompleteHandle()
    {
        _jobHandle = null;
        UpdateStatus(StatusState.Completed);
    }

    private IEnumerator Handle()
    {
        while (Status == StatusState.Entered && CanHandle())
        {
            Work();
            yield return _waitForSeconds;
        }

        CompleteHandle();
    }

    private void UpdateStatus(StatusState sateReadiness)
    {
        Status = sateReadiness;
    }
}