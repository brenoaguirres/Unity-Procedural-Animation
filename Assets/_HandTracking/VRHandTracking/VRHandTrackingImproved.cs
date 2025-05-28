using UnityEngine;
using UnityEngine.Animations.Rigging;

public class VRHandTrackingImproved : MonoBehaviour
{
    public enum HandSide
    {
        LEFT,
        RIGHT
    }
    public enum RigState
    {
        ACTIVATING,
        ACTIVE,
        DEACTIVATING,
        DEACTIVATED,
    }

    [Header("VR Hand Tracking Settings")]
    public HandSide handSide = HandSide.LEFT;
    public Transform vrController;
    public Transform ikTarget;
    [Range(0f, 1f)] public float positionSmoothness = 0.1f;
    [Range(0f, 1f)] public float rotationSmoothness = 0.1f;

    [Header("Offset Settings")]
    public Vector3 positionOffset = Vector3.zero;
    public Vector3 rotationOffset = Vector3.zero;

    [Header("VR Hand Tracking Settings")]
    public bool updateTracking = false;

    [Header("VR Hand Limits Confiner")]
    public BoxCollider confiner;
    public RigState rigState = RigState.DEACTIVATED;


    [Header("Animation Rigging IK Settings")]
    public TwoBoneIKConstraint ikConstraint;

    [Header("VR Rig Root")]
    public Transform vrRigRoot;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateState();

        if (updateTracking)
            UpdateTrackingPosition();
    }

    #region CUSTOM METHODS
    private void Initialize()
    {
        if (vrController == null || ikTarget == null)
        {
            Debug.LogWarning("VR Controller or IK Target not assigned!");
            return;
        }
    }
    private void UpdateTrackingPosition()
    {
        // Convert controller position to rig root local space
        Vector3 localControllerPosition = vrRigRoot.InverseTransformPoint(vrController.position);

        // Apply position offset in local space
        Vector3 localTargetPosition = localControllerPosition + positionOffset;

        // Convert back to world space
        Vector3 targetPosition = vrRigRoot.TransformPoint(localTargetPosition);

        // Interpolate position
        ikTarget.position = Vector3.Lerp(ikTarget.position, targetPosition, 1f - positionSmoothness);

        // Rotation handling remains the same
        Quaternion worldToLocal = Quaternion.Inverse(vrRigRoot.rotation);
        Quaternion relativeControllerRotation = worldToLocal * vrController.rotation;

        Quaternion offsetRotation = Quaternion.Euler(rotationOffset);
        Quaternion targetRotation = vrRigRoot.rotation * (relativeControllerRotation * offsetRotation);

        ikTarget.rotation = Quaternion.Lerp(ikTarget.rotation, targetRotation, 1f - rotationSmoothness);

        ClampBoundaries();
    }


    private void UpdateState()
    {
        switch (rigState)
        {
            default:
                break;
            case RigState.ACTIVATING:
                if (ikConstraint.weight < 1f)
                {
                    ikConstraint.weight = Mathf.Clamp01(ikConstraint.weight + Time.deltaTime);
                }
                else
                {
                    ActivateRig();
                }
                break;
            case RigState.DEACTIVATING:
                if (ikConstraint.weight > 0f)
                {
                    ikConstraint.weight = Mathf.Clamp01(ikConstraint.weight - Time.deltaTime);
                }
                else
                {
                    DeactivateRig();
                }
                break;
        }
    }

    public void ToggleHandRig(bool toggle)
    {
        if (toggle && rigState == RigState.DEACTIVATED)
        {
            rigState = RigState.ACTIVATING;
        }
        else if (!toggle && rigState == RigState.ACTIVE)
        {
            rigState = RigState.DEACTIVATING;
        }
    }
    public void ActivateRig()
    {
        GetCurrentOffset();
        updateTracking = true;
        ikConstraint.weight = 1f;
        rigState = RigState.ACTIVE;
    }
    public void DeactivateRig()
    {
        updateTracking = false;
        ikConstraint.weight = 0f;
        rigState = RigState.DEACTIVATED;
        CleanCurrentOffset();
    }
    public void GetCurrentOffset()
    {
        positionOffset = ikTarget.position - vrController.position;

        // Improved rotation offset calculation
        Quaternion worldToLocal = Quaternion.Inverse(vrRigRoot.rotation);
        Quaternion relativeControllerRotation = worldToLocal * vrController.rotation;
        Quaternion relativeIKTargetRotation = worldToLocal * ikTarget.rotation;

        Quaternion relativeRotation = Quaternion.Inverse(relativeControllerRotation) * relativeIKTargetRotation;
        rotationOffset = relativeRotation.eulerAngles;
    }

    public void CleanCurrentOffset()
    {
        positionOffset = Vector3.zero;
        rotationOffset = Vector3.zero;
    }

    public void ClampBoundaries()
    {
        if (confiner == null) return;

        Vector3 localPos = confiner.transform.InverseTransformPoint(ikTarget.position);

        Vector3 halfSize = confiner.size * 0.5f;

        localPos.x = Mathf.Clamp(localPos.x, -halfSize.x, halfSize.x);
        localPos.y = Mathf.Clamp(localPos.y, -halfSize.y, halfSize.y);
        localPos.z = Mathf.Clamp(localPos.z, -halfSize.z, halfSize.z);

        ikTarget.position = confiner.transform.TransformPoint(localPos);
    }
    #endregion

    #if UNITY_EDITOR
    private void OnValidate()
    {
        positionSmoothness = Mathf.Clamp01(positionSmoothness);
        rotationSmoothness = Mathf.Clamp01(rotationSmoothness);
    }
    #endif
}
