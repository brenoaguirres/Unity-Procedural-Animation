using UnityEngine;

public class TouchState : EnvironmentInteractionState
{
    float _elapsedTime = 0.0f;
    float _resetThreshold = .5f;

    public TouchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState
        stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState()
    {
        _elapsedTime = 0.0f;
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
    }

    public override void ExitState()
    {
        
    }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        if (_elapsedTime > _resetThreshold)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Reset;
        }

        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        StartIKTargetPositionTracking(other);
    }
    public override void OnTriggerStay(Collider other)
    {
        UpdateIKTargetPosition(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        ResetIKTargetPositionTracking(other);
    }
}
