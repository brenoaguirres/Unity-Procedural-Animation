using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class EnvironmentInteractionStateMachine : StateMachine<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset
    }

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void Awake()
    {
        ValidateConstraints();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK Constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right IK Constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left Multi Rotation Constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right Multi Rotation Constraint is not assigned.");
        Assert.IsNotNull(_rigidbody, "Rigidbody is not assigned.");
        Assert.IsNotNull(_rootCollider, "Root Collider is not assigned.");
    }
}
