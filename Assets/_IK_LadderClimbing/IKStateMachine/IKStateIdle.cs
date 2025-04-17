using UnityEngine;

public class IKStateIdle : IKState
{
    public IKStateIdle(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }

    public override void EnterState()
    {
        Debug.Log("Enter IDLE State");
        Context.EnableLocomotion();
        Context.DisableAllIKImmediate();
    }
    public override void UpdateState() { }

    public override void ExitState() { }

    public override IKStateMachine.EState GetNextState()
    {
        if (Context.CurrentIntersectingLadder != null && CheckWithinHangRange()) return IKStateMachine.EState.Hanging;

        if (Context.RootRigidbody.linearVelocity != Vector3.zero) return IKStateMachine.EState.Walking;

        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) 
    { 
        StartLadderPositionTracking(other);
    }

    public override void OnTriggerStay(Collider other) 
    {
        UpdateLadderPositionTracking(other);
    }

    public override void OnTriggerExit(Collider other) 
    {
        ResetLadderPositionTracking(other);
    }
}
