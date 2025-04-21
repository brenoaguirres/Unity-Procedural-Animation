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
        StartLadderHangPositions();
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
        if (CanClimb)
        {
            //ResetLadderHangPositions();
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
        float distanceThreshold = 0.4f;

        if (LeftHandConnected || Vector3.Distance(Context.LeftHandIKConstraint.data.target.transform.position,
                Context.NextStepLeft.transform.position) <= distanceThreshold)
        {
            LeftHandConnected = true;
            Context.LeftHandIKConstraint.data.target.transform.position =
                Context.NextStepLeft.transform.position;
        }

        if (RightHandConnected || Vector3.Distance(Context.RightHandIKConstraint.data.target.transform.position,
                Context.NextStepRight.transform.position) <= distanceThreshold)
        {
            RightHandConnected = true;
            Context.RightHandIKConstraint.data.target.transform.position =
                Context.NextStepRight.transform.position;
        }
    }

    public void StartLadderHangPositions()
    {
        LadderStep closestLadderStep = null;
        int index = 0;
        int closestIndex = 0;

        foreach (LadderStep step in Context.LadderStepsLeft)
        {
            if (closestLadderStep == null)
            {
                closestLadderStep = step;
                closestIndex = index;
            }

            if (Vector3.Distance(Context.LeftHandIKConstraint.data.target.transform.position,
                step.transform.position) <
                Vector3.Distance(Context.LeftHandIKConstraint.data.target.transform.position,
                closestLadderStep.transform.position))
            {
                closestLadderStep = step;
                closestIndex = index;
            }

            index++;
        }

        Context.HangStepLeft = closestLadderStep;
        Context.HangStepLeftIndex = closestIndex;
        Context.NextStepLeft = (Context.HangStepLeftIndex + 1 >= Context.LadderStepsLeft.Count)
            ? null : Context.LadderStepsLeft[Context.HangStepLeftIndex + 1];

        closestLadderStep = null;
        index = 0;
        closestIndex = 0;

        foreach (LadderStep step in Context.LadderStepsRight)
        {
            if (closestLadderStep == null)
            {
                closestLadderStep = step;
                closestIndex = index;
            }

            if (Vector3.Distance(Context.RightHandIKConstraint.data.target.transform.position,
                step.transform.position) <
                Vector3.Distance(Context.RightHandIKConstraint.data.target.transform.position,
                closestLadderStep.transform.position))
            {
                closestLadderStep = step;
                closestIndex = index;
            }

            index++;
        }

        Context.HangStepRight = closestLadderStep;
        Context.HangStepRightIndex = closestIndex;
        Context.NextStepRight = (Context.HangStepRightIndex + 1 >= Context.LadderStepsRight.Count)
            ? null : Context.LadderStepsRight[Context.HangStepRightIndex + 1];
    }

    public void ResetLadderHangPositions()
    {
        Context.HangStepLeft = null;
        Context.HangStepRight = null;
        Context.NextStepRight = null;
        Context.NextStepLeft = null;
        Context.HangStepRightIndex = 0;
        Context.HangStepLeftIndex = 0;
    }

    public void TestInputs()
    {
        float ms = Time.deltaTime * 0.3f;

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
