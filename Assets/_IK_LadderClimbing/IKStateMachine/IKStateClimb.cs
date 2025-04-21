using UnityEngine;

public class IKStateClimb : IKState
{
    public float ClimbSpeed = 0.5f;
    public int RemainingSteps;

    public bool RightSideClimb = true;
    public Vector3 StepStartingPosition;
    public float LerpPosition = 0f;
    public float LerpSpeed = 0.5f;

    public IKStateClimb(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }
    public override void EnterState() 
    {
        Debug.Log("Enter CLIMB State");
        StepStartingPosition = Vector3.zero;
        RemainingSteps = Context.StepsPerClimb;
    }
    public override void UpdateState() 
    { 
        StartClimbStep();
        UpdateClimbStep();
        FinishClimbStep();
        //UpdateClimberPosition();
    }

    public override void ExitState() { }

    public override IKStateMachine.EState GetNextState()
    {
        if (RemainingSteps <= 0)
        {
            return IKStateMachine.EState.Hanging;
        }

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

    public void StartClimbStep()
    {
        if (StepStartingPosition != Vector3.zero) return;

        Debug.Log("Here");
        if (RightSideClimb)
        {
            StepStartingPosition = Context.RightHandIKConstraint.data.target.position;
        }
        else
        {
            StepStartingPosition = Context.LeftHandIKConstraint.data.target.position;
        }
    }

    public void UpdateClimbStep()
    {
        if (RightSideClimb)
        {
            Context.RightHandIKConstraint.data.target.position =
                Vector3.Lerp(StepStartingPosition, Context.NextStepRight.transform.position, LerpPosition);
            LerpPosition += Time.deltaTime / LerpSpeed;
        }
        else
        {
            Context.RightHandIKConstraint.data.target.position =
                Vector3.Lerp(StepStartingPosition, Context.NextStepLeft.transform.position, LerpPosition);
            LerpPosition += Time.deltaTime / LerpSpeed;
        }
    }

    public void FinishClimbStep()
    {
        if (LerpPosition < 1f) return;

        if (RightSideClimb)
        {
            // Assign Context.HangStepRight to current closest step
            // Assign Context.HangStepRightIndex
            // Assign Context.NextStepRight to +2
        }
        else
        {
            // Same to left side
        }
        
        RightSideClimb = !RightSideClimb;
        LerpPosition = 0f;
        StepStartingPosition = Vector3.zero;
        RemainingSteps--;
    }
}
