using UnityEngine;

public class IKStateHang : IKState
{
    private bool CanClimb;

    public IKStateHang(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }
    public override void EnterState() 
    {
        Debug.Log("Enter HANG State");
        Context.DisableLocomotion();
        Context.EnableAllIKImmediate();
        CanClimb = true;
    }
    public override void UpdateState() { }

    public override void ExitState() { }

    public override IKStateMachine.EState GetNextState()
    {
        if (CanClimb) return IKStateMachine.EState.Climbing;

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
