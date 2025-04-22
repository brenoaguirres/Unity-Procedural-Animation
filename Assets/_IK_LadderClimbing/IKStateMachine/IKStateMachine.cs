using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class IKStateMachine : StateMachine<IKStateMachine.EState>
{
    public enum EState
    {
        Idle,
        Walking,
        Climbing,
        Hanging,
    }

    private IKContext _context;

    [Header("Rig Constraints")]
    [SerializeField] private TwoBoneIKConstraint _leftHandIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightHandIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _leftFootIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightFootIKConstraint;
    [SerializeField] private MultiAimConstraint _headMultiAimConstraint;
    [SerializeField] private MultiParentConstraint _hipsMultiParentConstraint;
    [SerializeField] private Transform _hipsTarget;
    [Space(1)]
    [Header("Components")]
    [SerializeField] private Rigidbody _rootRigidbody;
    [SerializeField] private CapsuleCollider _rootCollider;
    [SerializeField] private CharacterLocomotion _characterLocomotion;
    [Space(1)]
    [Header("Inputs")]
    [SerializeField] private Vector3 _input3D = Vector3.zero;
    [SerializeField] private bool _inputButton = false;

    private void Awake()
    {
        ValidateConstraints();
        InitializeContext();
        InitializeStates();
        ConstructEnvironmentDetectionCollider();
    }

    protected override void UpdateInputs()
    {
        if (_context == null) return;

        _context.Input3D = _input3D.normalized;
        _context.InputButton = _inputButton;
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftHandIKConstraint, "Left Hand IK Constraint not assigned in inspector.");
        Assert.IsNotNull(_rightHandIKConstraint, "Right Hand IK Constraint not assigned in inspector.");
        Assert.IsNotNull(_leftFootIKConstraint, "Left Foot IK Constraint not assigned in inspector.");
        Assert.IsNotNull(_rightFootIKConstraint, "Right Foot IK Constraint not assigned in inspector.");
        Assert.IsNotNull(_headMultiAimConstraint, "Head Multi Aim Constraint not assigned in inspector.");
        Assert.IsNotNull(_hipsMultiParentConstraint, "Hips Multi Parent Constraint not assigned in inspector.");
    }

    private void InitializeContext()
    {
        _context = new IKContext(_leftHandIKConstraint, _rightHandIKConstraint, _leftFootIKConstraint, _rightFootIKConstraint,
            _headMultiAimConstraint, _hipsMultiParentConstraint, _rootRigidbody, _rootCollider, _characterLocomotion, _hipsTarget);
    }

    private void InitializeStates()
    {
        // Add States to inherited StateManager "States" dictionary and Set Initial State.
        States.Add(EState.Idle, new IKStateIdle(_context, EState.Idle));
        States.Add(EState.Walking, new IKStateWalk(_context, EState.Walking));
        States.Add(EState.Climbing, new IKStateClimb(_context, EState.Climbing));
        States.Add(EState.Hanging, new IKStateHang(_context, EState.Hanging));

        CurrentState = States[EState.Idle];
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        // a character's wingspan is very close to its height
        float wingspan = _rootCollider.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_rootCollider.center.x,
            (_rootCollider.center.y + (0.25f * wingspan)), _rootCollider.center.z + (0.5f * wingspan));
        boxCollider.isTrigger = true;
    }
}
