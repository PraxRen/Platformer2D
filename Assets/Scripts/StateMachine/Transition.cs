using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public bool NeedTransit { get; protected set; }
    public State CurrentState { get; private set; }
    public State TargetState => _targetState;
    protected AIEnemyController AIController { get; private set; }

    public void Initialize(AIEnemyController aiController, State currentState)
    {
        RunActionBeforeInitialize(aiController, currentState);
        AIController = aiController;
        CurrentState = currentState;
        RunActionAfterInitialize(aiController, currentState);
    }

    public virtual void Activate() { }

    public void Deactivate() 
    {
        RunActionBeforeDeactivate();
        NeedTransit = false;
        RunActionAfterDeactivate();
    } 

    protected virtual void RunActionBeforeInitialize(AIEnemyController aiController, State currentState) { }

    protected virtual void RunActionAfterInitialize(AIEnemyController aiController, State currentState) { }

    protected virtual void RunActionBeforeDeactivate() { }

    protected virtual void RunActionAfterDeactivate() { }
}
