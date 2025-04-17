using UnityEngine;

public class IKStateClimb : IKState
{
    public float ClimbSpeed = 2.0f;
    public IKStateClimb(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }
    public override void EnterState() 
    {
        Debug.Log("Enter CLIMB State");
    }
    public override void UpdateState() 
    { 
        UpdateClimberPosition();
    }

    public override void ExitState() { }

    public override IKStateMachine.EState GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    public override void OnTriggerExit(Collider other) { }

    public void UpdateClimberPosition()
    {
        if (Mathf.Abs(Context.Input3D.y) > 0.01f)
        {
            Context.RootRigidbody.linearVelocity = new Vector3(0, Context.Input3D.y * ClimbSpeed, 0);
        }
    }
}
