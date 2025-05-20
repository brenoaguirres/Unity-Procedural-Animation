using UnityEngine;
using UnityEngine.Animations.Rigging;

public class VRHandTracking : MonoBehaviour
{
    public enum RigState
    {
        ACTIVATING,
        ACTIVE,
        DEACTIVATING,
        DEACTIVATED,
    }

    [Header("VR Hand Tracking Settings")]
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
        Vector3 targetPosition = vrController.position + vrController.TransformDirection(positionOffset);
        Quaternion targetRotation = vrController.rotation * Quaternion.Euler(rotationOffset);

        ikTarget.position = Vector3.Lerp(ikTarget.position, targetPosition, 1f - positionSmoothness);
        ikTarget.rotation = Quaternion.Lerp(ikTarget.rotation, targetRotation, 1f - rotationSmoothness);
        ClampBoundaries();
    }

    private void UpdateState()
    {
        switch(rigState)
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
    }
    public void GetCurrentOffset()
    {
        positionOffset = ikTarget.position - vrController.position;
        rotationOffset = Quaternion.Inverse(vrController.rotation) * ikTarget.rotation.eulerAngles;
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
}
