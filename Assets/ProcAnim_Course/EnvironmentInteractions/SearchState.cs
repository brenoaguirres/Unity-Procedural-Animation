using UnityEngine;

public class SearchState : EnvironmentInteractionState
{
    public float _approachDistanceThreshold = 2.0f;
    public SearchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState
        stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState() 
    {
        Debug.Log("ENTER SEARCH STATE");
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isCloseToTarget = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder,
            Context.RootTransform.position) < _approachDistanceThreshold;

        bool isClosestPointOnColliderValid = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

        if (isCloseToTarget && isClosestPointOnColliderValid)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Approach;
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
