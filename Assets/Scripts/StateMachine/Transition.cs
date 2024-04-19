using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public bool NeedTransit { get; protected set; }
    public State CurrentState { get; private set; }
    public State TargetState => _targetState;
    protected AIEnemy AIEnemy { get; private set; }

    public void Initialize(AIEnemy aiEnemy, State currentState)
    {
        RunActionBeforeInitialize(aiEnemy, currentState);
        AIEnemy = aiEnemy;
        CurrentState = currentState;
        RunActionAfterInitialize(aiEnemy, currentState);
    }

    public virtual void Activate() { }

    public void Deactivate() 
    {
        RunActionBeforeDeactivate();
        NeedTransit = false;
        RunActionAfterDeactivate();
    } 

    protected virtual void RunActionBeforeInitialize(AIEnemy aiEnemy, State currentState) { }

    protected virtual void RunActionAfterInitialize(AIEnemy aiEnemy, State currentState) { }

    protected virtual void RunActionBeforeDeactivate() { }

    protected virtual void RunActionAfterDeactivate() { }
}
