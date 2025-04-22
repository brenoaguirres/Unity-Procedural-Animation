using UnityEngine;

public class IKStateClimb : IKState
{
    public enum EClimbState
    {
        START,
        UPDATE,
        FINISH,
    }
    public float ClimbSpeed = 0.5f;
    public int RemainingSteps;

    public bool RightSideClimb = true;
    public Vector3 StepHandStartingPosition;
    public Vector3 StepFootStartingPosition;
    public float LerpPosition = 0f;
    public float LerpSpeed = 0.5f;

    public EClimbState ClimbState = EClimbState.START;

    public IKStateClimb(IKContext context, IKStateMachine.EState key) : base(context, key)
    {
        Context = context;
    }
    public override void EnterState()
    {
        Debug.Log("Enter CLIMB State");
        StepHandStartingPosition = Vector3.zero;
        RemainingSteps = Context.StepsPerClimb;
    }
    public override void UpdateState()
    {
        switch (ClimbState)
        {
            case EClimbState.START:
                ClimbState = StartClimbStep();
                break;
            case EClimbState.UPDATE:
                ClimbState = UpdateClimbStep();
                break;
            case EClimbState.FINISH:
                ClimbState = FinishClimbStep();
                break;
        }
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

    public EClimbState StartClimbStep()
    {
        Debug.Log("Start Climb");
        if (RightSideClimb)
        {
            StepHandStartingPosition = Context.RightHandIKConstraint.data.target.transform.position;
            StepFootStartingPosition = Context.LeftFootIKConstraint.data.target.transform.position;
        }
        else
        {
            StepHandStartingPosition = Context.LeftHandIKConstraint.data.target.transform.position;
            StepFootStartingPosition = Context.RightFootIKConstraint.data.target.transform.position;
        }

        Context.SetAllTargetSteps();
        return EClimbState.UPDATE;
    }

    public EClimbState UpdateClimbStep()
    {
        if (LerpPosition >= 1f) return EClimbState.FINISH;

        if (RightSideClimb)
        {
            Context.RightHandIKConstraint.data.target.transform.position =
                Vector3.Lerp(StepHandStartingPosition, Context.TargetRightHandStep.transform.position, LerpPosition);
            Context.LeftFootIKConstraint.data.target.transform.position =
                Vector3.Lerp(StepFootStartingPosition, Context.TargetLeftFootStep.transform.position, LerpPosition);
            LerpPosition += Time.deltaTime / LerpSpeed;
        }
        else
        {
            Context.LeftHandIKConstraint.data.target.transform.position =
                Vector3.Lerp(StepHandStartingPosition, Context.TargetLeftHandStep.transform.position, LerpPosition);
            Context.RightFootIKConstraint.data.target.transform.position =
                Vector3.Lerp(StepFootStartingPosition, Context.TargetRightFootStep.transform.position, LerpPosition);
            LerpPosition += Time.deltaTime / LerpSpeed;
        }

        return EClimbState.UPDATE;
    }

    public EClimbState FinishClimbStep()
    {
        RightSideClimb = !RightSideClimb;
        LerpPosition = 0f;
        StepHandStartingPosition = Vector3.zero;
        RemainingSteps--;

        return EClimbState.START;
    }
}
