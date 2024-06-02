using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : Character
{
    [SerializeField] private List<State> _states;

    private State _currentState;

    private void Start()
    {
        StartCoroutine(InitializeStates());
    }

    private void Update()
    {
        if (_currentState == null)
            return;

        if (_currentState.TryGetNextState(out State nextState) == false)
            return;

        Transit(nextState);
    }

    private IEnumerator InitializeStates()
    {
        if (_states == null || _states.Count == 0)
            yield break;

        foreach (State state in _states)
        {
            state.Initialize(this);
            yield return null;
        }

        SetCurrentState(_states[0]);
    }

    private void SetCurrentState(State startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }

    private void Transit(State nextState)
    {
        if (_states.Contains(nextState) == false)
            return;

        if (_currentState != null)
            _currentState.Exit();

        SetCurrentState(nextState);
    }

    protected override void HandleEnable()
    {
        if (_currentState != null)
            SetCurrentState(_currentState);
    }

    protected override void HandleDisable()
    {
        if (_currentState != null)
            _currentState.Exit();
    }
}