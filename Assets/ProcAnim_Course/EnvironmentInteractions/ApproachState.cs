using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    public float _approachWeight = 0.5f;
    public float _elapsedTime = 0f;
    public float _lerpDuration = 5.0f;
    public ApproachState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState
        stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("ENTER APPROACH STATE");
    }

    public override void UpdateState() 
    {
        _elapsedTime += Time.deltaTime;

        Context.CurrentIKConstraint.weight = Mathf.Lerp(Context.CurrentIKConstraint.weight, _approachWeight, 
            _elapsedTime / _lerpDuration);
    }
    public override void ExitState() { }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    public override void OnTriggerExit(Collider other) { }
}
