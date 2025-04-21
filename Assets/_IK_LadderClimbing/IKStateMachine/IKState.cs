using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public abstract class IKState : BaseState<IKStateMachine.EState>
{
    private readonly string LadderTag = "Ladder";

    protected IKContext Context;

    public IKState(IKContext context, IKStateMachine.EState
        stateKey) : base(stateKey)
    {
        Context = context;
    }

    protected void StartLadderPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider.gameObject.tag == LadderTag &&
            Context.CurrentIntersectingLadder == null)
        {
            Context.CurrentIntersectingLadder = intersectingCollider;
            Context.LadderStepsLeft = intersectingCollider.GetComponentsInChildren<LadderStep>(true)
                .ToList().FindAll(x => x.LadderStepSide == LadderStep.ELadderStepSide.L);
            Context.LadderStepsRight = intersectingCollider.GetComponentsInChildren<LadderStep>(true)
                .ToList().FindAll(x => x.LadderStepSide == LadderStep.ELadderStepSide.R);

            SetIKTargetPosition();
        }
    }

    protected void UpdateLadderPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentIntersectingLadder)
        {
            SetIKTargetPosition();
        }
    }

    protected void ResetLadderPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentIntersectingLadder)
        {
            Context.CurrentIntersectingLadder = null;
            Context.LadderStepsLeft.Clear();
            Context.LadderStepsRight.Clear();
        }
    }

    protected bool CheckWithinHangRange()
    {
        if (Context.CurrentLadderBase == Vector3.positiveInfinity) return false;

        bool withinRange = Vector3.Distance(Context.RootCollider.transform.position,
            Context.CurrentLadderBase) <= Context.HangMinimumDistanceFromLadder;
        bool withinAngle = Vector3.Dot(Context.RootCollider.transform.forward,
            Context.CurrentIntersectingLadder.transform.forward) >= Context.HangMinimumAngleFromLadder;

        return withinRange && withinAngle;
    }


    private void SetIKTargetPosition()
    {
        /*
        Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(Context.CurrentIntersectingCollider,
            new Vector3(Context.CurrentShoulderTransform.position.x, Context.CharacterShoulderHeight, Context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = .05f;
        Vector3 offset = normalizedRayDirection * offsetDistance;

        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIKTargetTransform.position = new Vector3(offsetPosition.x, Context.InteractionPointYOffset, offsetPosition.z);
        */
    }
}
