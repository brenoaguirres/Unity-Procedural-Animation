using UnityEngine;

public class IKStateWalk : IKState
{
    public IKStateWalk(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }
    public override void EnterState()
    {
        Debug.Log("Enter WALK State");
        Context.EnableLocomotion();
        Context.DisableAllIKImmediate();
    }
    public override void UpdateState() { }

    public override void ExitState() { }

    public override IKStateMachine.EState GetNextState()
    {
        if (Context.CurrentIntersectingLadder != null && CheckWithinHangRange()) return IKStateMachine.EState.Hanging;

        if (Context.RootRigidbody.linearVelocity == Vector3.zero) return IKStateMachine.EState.Idle;

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
