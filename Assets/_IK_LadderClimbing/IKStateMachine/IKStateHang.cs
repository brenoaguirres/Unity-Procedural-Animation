using System.Collections;
using UnityEngine;

public class IKStateHang : IKState
{
    private bool CanClimb;
    private bool LeftHandConnected;
    private bool RightHandConnected;

    private bool IsConnectedToLadder;
    private float _timeToStartRig = 0.8f;

    public IKStateHang(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }
    public override void EnterState() 
    {
        Debug.Log("Enter HANG State");
        StartRigBehaviour();

        LeftHandConnected = false;
        RightHandConnected = false;
        CanClimb = false;
    }
    public override void UpdateState() 
    {
        if (!IsConnectedToLadder)
        {
            IsConnectedToLadder = UpdateRigBehaviour();
            return;
        }

        TestInputs();
        CheckHandsConnected();
        if (LeftHandConnected && RightHandConnected) CanClimb = true;
    }

    public override void ExitState() { }

    public override IKStateMachine.EState GetNextState()
    {
        if (Context.CheckLastStep())
        {
            // Return new state
        }

        if (CanClimb)
        {
            AssignLadderClimbPositions();
            Context.IsStartingLadderInteraction = false;
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
            Context.RightFootIKConstraint.data.target.transform.position =
                Context.TargetRightFootStep.transform.position;
        }

        if (!LeftHandConnected) return;

        if (RightHandConnected || Vector3.Distance(Context.RightHandIKConstraint.data.target.transform.position,
                Context.TargetRightHandStep.transform.position) <= distanceThreshold)
        {
            RightHandConnected = true;
            Context.RightHandIKConstraint.data.target.transform.position =
                Context.TargetRightHandStep.transform.position;
            Context.LeftFootIKConstraint.data.target.transform.position =
                Context.TargetLeftFootStep.transform.position;
        }
    }

    public void StartRigBehaviour()
    {
        if (Context.IsStartingLadderInteraction)
        {
            Context.DisableLocomotion();
            Context.AddRotationToHips();
            Context.SetAllStartingSteps();
        }
    }

    public bool UpdateRigBehaviour()
    {
        if (Context.IsStartingLadderInteraction)
        {
            Context.Rig.weight = Mathf.Lerp(0f, 1f, Time.deltaTime / _timeToStartRig);
        }

        return Context.Rig.weight >= 1f;
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
            Context.RightFootIKConstraint.data.target.transform.position +=
                new Vector3(1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(-1 * ms, 0f, 0f);
            Context.RightFootIKConstraint.data.target.transform.position +=
                new Vector3(-1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 1 * ms, 0f);
            Context.RightFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, 1 * ms, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, -1 * ms, 0f);
            Context.RightFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, -1 * ms, 0f);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, 1 * ms);
            Context.RightFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, 1 * ms);
        }
        if (Input.GetKey(KeyCode.X))
        {
            Context.LeftHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, -1 * ms);
            Context.RightFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, -1 * ms);
        }

        if (Input.GetKey(KeyCode.Y))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(1 * ms, 0f, 0f);
            Context.LeftFootIKConstraint.data.target.transform.position +=
                new Vector3(1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.U))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(-1 * ms, 0f, 0f);
            Context.LeftFootIKConstraint.data.target.transform.position +=
                new Vector3(-1 * ms, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.H))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 1f * ms, 0f);
            Context.LeftFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, 1f * ms, 0f);
        }
        if (Input.GetKey(KeyCode.J))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, -1f * ms, 0f);
            Context.LeftFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, -1f * ms, 0f);
        }
        if (Input.GetKey(KeyCode.N))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, 1 * ms);
            Context.LeftFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, 1 * ms);
        }
        if (Input.GetKey(KeyCode.M))
        {
            Context.RightHandIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, -1 * ms);
            Context.LeftFootIKConstraint.data.target.transform.position +=
                new Vector3(0f, 0f, -1 * ms);
        }
    }
}
