using UnityEngine;

public class IKStateHang : IKState
{
    private bool CanClimb;
    private bool LeftHandConnected;
    private bool RightHandConnected;

    public IKStateHang(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }
    public override void EnterState() 
    {
        Debug.Log("Enter HANG State");
        Context.DisableLocomotion();
        Context.EnableAllIKImmediate();
        AssignLadderClimbPositions();
        LeftHandConnected = false;
        RightHandConnected = false;
        CanClimb = false;
    }
    public override void UpdateState() 
    {
        TestInputs();
        CheckHandsConnected();

        if (LeftHandConnected && RightHandConnected) CanClimb = true;
    }

    public override void ExitState() { }

    public override IKStateMachine.EState GetNextState()
    {
        if (Context.CheckLastStep())
        {
            // change later to exit climb state
            Debug.Log("Im in last step of ladder, exiting climb state");
            //return IKStateMachine.EState.Idle;
        }

        if (CanClimb)
        {
            AssignLadderClimbPositions();
            return IKStateMachine.EState.Climbing;
        }

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

    public void CheckHandsConnected()
    {
        float distanceThreshold = 0.2f;

        if (LeftHandConnected || Vector3.Distance(Context.LeftHandIKConstraint.data.target.transform.position,
                Context.TargetLeftHandStep.transform.position) <= distanceThreshold)
        {
            LeftHandConnected = true;
            Context.LeftHandIKConstraint.data.target.transform.position =
                Context.TargetLeftHandStep.transform.position;
        }

        if (RightHandConnected || Vector3.Distance(Context.RightHandIKConstraint.data.target.transform.position,
                Context.TargetRightHandStep.transform.position) <= distanceThreshold)
        {
            RightHandConnected = true;
            Context.RightHandIKConstraint.data.target.transform.position =
                Context.TargetRightHandStep.transform.position;
        }
    }

    public void AssignLadderClimbPositions()
    {
        Context.SetAllTargetSteps();
    }

    public void TestInputs()
    {
        float ms = Time.deltaTime * 0.7f;

        if (Input.GetKey(KeyCode.Q))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(-1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 1 * ms, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, -1 * ms, 0f);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, 1 * ms);
        }
        if (Input.GetKey(KeyCode.X))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, -1 * ms);
        }

        if (Input.GetKey(KeyCode.Y))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.U))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(-1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.H))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 1 * ms, 0f);
        }
        if (Input.GetKey(KeyCode.J))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, -1 * ms, 0f);
        }
        if (Input.GetKey(KeyCode.N))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, 1 * ms);
        }
        if (Input.GetKey(KeyCode.M))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, -1 * ms);
        }
    }
}
