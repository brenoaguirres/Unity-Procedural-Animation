using UnityEngine;

public class ApproachState : EnvironmentInteractionState
{
    public ApproachState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState
        stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("ENTER APPROACH STATE");
    }

    public override void UpdateState() { }
    public override void ExitState() { }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    public override void OnTriggerExit(Collider other) { }
}
