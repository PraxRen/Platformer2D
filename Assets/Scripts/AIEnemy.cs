using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Fighter), typeof(Health))]
public class AIEnemy : MonoBehaviour
{
    [SerializeField] private List<State> _states;

    private Mover _mover;
    private Fighter _fighter;
    private Health _health;
    private State _currentState;

    private void OnEnable()
    {
        if (_health != null)
            _health.Died += OnDied;

        if (_currentState != null)
            SetCurrentState(_currentState);
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;

        if (_currentState != null)
            _currentState.Exit();
    }

    private void Start()
    {
        _mover = GetComponent<Mover>();
        _fighter = GetComponent<Fighter>();
        _health = GetComponent<Health>();
        _health.Died += OnDied;
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

    private void OnDied()
    {
        enabled = false;
        _mover.enabled = false;
        _fighter.enabled = false;
        _health.enabled = false;
    }
}