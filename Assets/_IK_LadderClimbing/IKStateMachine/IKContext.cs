using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKContext
{

    public IKContext(TwoBoneIKConstraint leftHandIK, TwoBoneIKConstraint rightHandIK, TwoBoneIKConstraint leftFootIK, TwoBoneIKConstraint rightFootIK,
        MultiAimConstraint headMultiAim, MultiParentConstraint hipsMultiParent, Rigidbody rootRigidbody, CapsuleCollider rootCollider,
        CharacterLocomotion characterLocomotion)
    {
        _leftHandIKConstraint = leftHandIK;
        _rightHandIKConstraint = rightHandIK;
        _leftFootIKConstraint = leftFootIK;
        _rightFootIKConstraint = rightFootIK;
        _headMultiAimConstraint = headMultiAim;
        _hipsMultiParentConstraint = hipsMultiParent;
        _rootRigidbody = rootRigidbody;
        _rootCollider = rootCollider;
        _characterLocomotion = characterLocomotion;
    }

    private TwoBoneIKConstraint _leftHandIKConstraint;
    private TwoBoneIKConstraint _rightHandIKConstraint;
    private TwoBoneIKConstraint _leftFootIKConstraint;
    private TwoBoneIKConstraint _rightFootIKConstraint;
    private MultiAimConstraint _headMultiAimConstraint;
    private MultiParentConstraint _hipsMultiParentConstraint;
    private Rigidbody _rootRigidbody;
    private CapsuleCollider _rootCollider;
    private CharacterLocomotion _characterLocomotion;
    private Collider _currentIntersectingLadder;

    public TwoBoneIKConstraint LeftHandIKConstraint => _leftHandIKConstraint;
    public TwoBoneIKConstraint RightHandIKConstraint => _rightHandIKConstraint;
    public TwoBoneIKConstraint LeftFootIKConstraint => _leftFootIKConstraint;
    public TwoBoneIKConstraint RightFootIKConstraint => _rightFootIKConstraint;
    public MultiAimConstraint HeadMultiAimConstraint => _headMultiAimConstraint;
    public MultiParentConstraint HipsMultiParentConstraint => _hipsMultiParentConstraint;
    public Rigidbody RootRigidbody => _rootRigidbody;
    public CapsuleCollider RootCollider => _rootCollider;
    public CharacterLocomotion CharacterLocomotion => _characterLocomotion;

    public Collider CurrentIntersectingLadder
    {
        get => _currentIntersectingLadder;
        set
        {
            if (value == null) CurrentLadderBase = Vector3.positiveInfinity;
            _currentIntersectingLadder = value;
            
            if (_currentIntersectingLadder != null) CurrentLadderBase = GetLadderBase();
            
        }
    }
    public Vector3 CurrentLadderBase = Vector3.positiveInfinity;
    public float HangMinimumDistanceFromLadder = 0.4f;
    public float HangMinimumAngleFromLadder = 0.3f;

    public List<LadderStep> LadderStepsRight = new List<LadderStep>();
    public List<LadderStep> LadderStepsLeft = new List<LadderStep>();

    public LadderStep CurrentLeftHandStep;
    public LadderStep CurrentRightHandStep;
    public LadderStep CurrentLeftFootStep;
    public LadderStep CurrentRightFootStep;
    public LadderStep TargetLeftHandStep;
    public LadderStep TargetRightHandStep;
    public LadderStep TargetLeftFootStep;
    public LadderStep TargetRightFootStep;

    public LadderStep HangStepRight;
    public LadderStep HangStepLeft;
    public int HangStepRightIndex = 0;
    public int HangStepLeftIndex = 0;
    public LadderStep NextStepRight;
    public LadderStep NextStepLeft;

    public int StepsPerClimb = 2;


    public Vector3 Input3D = Vector3.zero;
    public bool InputButton = false;

    public void EnableAllIKImmediate()
    {
        LeftHandIKConstraint.weight = 1f;
        RightHandIKConstraint.weight = 1f;
        LeftFootIKConstraint.weight = 1f;
        RightFootIKConstraint.weight = 1f;
        HeadMultiAimConstraint.weight = 1f;
        HipsMultiParentConstraint.weight = 1f;
    }

    public void DisableAllIKImmediate()
    {
        LeftHandIKConstraint.weight = 0f;
        RightHandIKConstraint.weight = 0f;
        LeftFootIKConstraint.weight = 0f;
        RightFootIKConstraint.weight = 0f;
        HeadMultiAimConstraint.weight = 0f;
        HipsMultiParentConstraint.weight = 0f;
    }

    public void EnableLocomotion()
    {
        RootRigidbody.useGravity = true;
        CharacterLocomotion.enabled = true;
    }

    public void DisableLocomotion()
    {
        RootRigidbody.useGravity = false;
        CharacterLocomotion.enabled = false;
    }

    public Vector3 GetLadderBase()
    {
        return new Vector3(CurrentIntersectingLadder.transform.position.x,
            CurrentIntersectingLadder.transform.position.y - CurrentIntersectingLadder.bounds.size.y / 2,
            CurrentIntersectingLadder.transform.position.z);
    }

    public LadderStep GetClosestLadderStepToIKBone(TwoBoneIKConstraint bone, List<LadderStep> steps)
    {
        LadderStep closestLadderStep = null;
        int index = 0;
        int closestIndex = 0;

        foreach (LadderStep step in steps)
        {
            if (closestLadderStep == null)
            {
                closestLadderStep = step;
                closestIndex = index;
            }

            if (Vector3.Distance(bone.data.target.transform.position,
                step.transform.position) <
                Vector3.Distance(bone.data.target.transform.position,
                closestLadderStep.transform.position))
            {
                closestLadderStep = step;
                closestIndex = index;
            }

            index++;
        }

        return closestLadderStep;
    }

    public LadderStep GetNextLadderStep(LadderStep step, List<LadderStep> steps)
    {
        if (steps.Contains(step)) return steps[steps.IndexOf(step) + 1];
        else
        {
            Debug.LogError("Next step not found in list of steps.");
            return null;
        }
    }

    public bool IsLastStep(LadderStep step, List<LadderStep> steps)
    {
        return steps.IndexOf(step) + 1 > steps.Count - 1;
    }

    public void SetAllTargetSteps()
    {
        CurrentLeftHandStep = GetClosestLadderStepToIKBone(LeftHandIKConstraint, LadderStepsLeft);
        CurrentRightHandStep = GetClosestLadderStepToIKBone(RightHandIKConstraint, LadderStepsRight);
        CurrentLeftFootStep = GetClosestLadderStepToIKBone(LeftFootIKConstraint, LadderStepsLeft);
        CurrentRightFootStep = GetClosestLadderStepToIKBone(RightFootIKConstraint, LadderStepsRight);

        TargetLeftHandStep = GetNextLadderStep(CurrentLeftHandStep, LadderStepsLeft);
        TargetRightHandStep = GetNextLadderStep(CurrentRightHandStep, LadderStepsRight);
        TargetLeftFootStep = GetNextLadderStep(CurrentLeftFootStep, LadderStepsLeft);
        TargetRightFootStep = GetNextLadderStep(CurrentRightFootStep, LadderStepsRight);
    }
}
