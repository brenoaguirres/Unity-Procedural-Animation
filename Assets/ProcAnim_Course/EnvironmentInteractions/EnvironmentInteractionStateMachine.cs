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

    private EnvironmentInteractionContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;

    private void Awake()
    {
        ValidateConstraints();

        _context = new EnvironmentInteractionContext(_leftIkConstraint, _rightIkConstraint,
            _leftMultiRotationConstraint, _rightMultiRotationConstraint, _rigidbody, _rootCollider,
            transform.root);

        InitializeStates();
        ConstructEnvironmentDetectionCollider();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
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

    private void InitializeStates()
    {
        // Add States to inherited StateManager "States" dictionary and Set Initial State.
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));

        CurrentState = States[EEnvironmentInteractionState.Reset];
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        // a character's wingspan is very close to its height
        float wingspan = _rootCollider.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_rootCollider.center.x, 
            (_rootCollider.center.y + (0.25f * wingspan)) - _rootCollider.center.y, _rootCollider.center.z + (0.5f * wingspan));
        boxCollider.isTrigger = true;

        _context.ColliderCenterY = _rootCollider.center.y;
    }
}
