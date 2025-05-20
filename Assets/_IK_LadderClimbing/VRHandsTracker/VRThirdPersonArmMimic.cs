using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
public class VRThirdPersonArmMimic : MonoBehaviour
{
    [System.Serializable]
    public class ArmSettings
    {
        public Transform vrController;
        public Transform armTarget;
        public Transform wristBone;
        public TwoBoneIKConstraint ikConstraint;

        [Header("Auto-Calibration")]
        public bool autoCalibrateOffsets = true;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        [Header("Runtime Adjustments")]
        [Range(0f, 1f)] public float mimicAmount = 1f;
        public Vector3 positionAdjustment = Vector3.zero;
        public Vector3 rotationAdjustment = Vector3.zero;

        [HideInInspector] public Vector3 initialPosition;
        [HideInInspector] public Quaternion initialRotation;
    }

    [Header("Arm Setup")]
    public ArmSettings leftArm;
    public ArmSettings rightArm;

    [Header("Settings")]
    public bool useIK = true;
    public float calibrationDuration = 1f;
    public bool showDebugGizmos = true;

    private Animator animator;
    private bool isCalibrated;
    private float calibrationTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Initialize arm settings
        InitArm(ref leftArm);
        InitArm(ref rightArm);

        // Set initial IK weights
        SetIKWeights();
    }

    private void InitArm(ref ArmSettings arm)
    {
        if (arm.armTarget != null)
        {
            arm.initialPosition = arm.armTarget.localPosition;
            arm.initialRotation = arm.armTarget.localRotation;
        }

        if (arm.autoCalibrateOffsets && arm.vrController != null && arm.wristBone != null)
        {
            CalibrateArmOffsets(ref arm);
        }
    }

    private void CalibrateArmOffsets(ref ArmSettings arm)
    {
        // Calculate position offset (controller to wrist)
        arm.positionOffset = arm.wristBone.position - arm.vrController.position;

        // Calculate rotation offset (difference between controller and wrist rotation)
        arm.rotationOffset = (Quaternion.Inverse(arm.vrController.rotation) * arm.wristBone.rotation).eulerAngles;

        Debug.Log($"Calibrated offsets - Position: {arm.positionOffset}, Rotation: {arm.rotationOffset}");
    }

    private void Update()
    {
        // Handle calibration phase
        if (!isCalibrated)
        {
            calibrationTimer += Time.deltaTime;
            if (calibrationTimer >= calibrationDuration)
            {
                isCalibrated = true;
            }
        }

        // Update arm positions
        UpdateArm(leftArm);
        UpdateArm(rightArm);
    }

    private void UpdateArm(ArmSettings arm)
    {
        if (arm.vrController == null || arm.armTarget == null) return;

        if (arm.autoCalibrateOffsets && !isCalibrated && arm.wristBone != null)
        {
            // During calibration, continuously update offsets for smooth transition
            CalibrateArmOffsets(ref arm);
        }

        // Calculate target position with all offsets
        Vector3 targetPosition = arm.vrController.position
                               + arm.vrController.TransformDirection(arm.positionOffset + arm.positionAdjustment);

        // Calculate target rotation with all offsets
        Quaternion targetRotation = arm.vrController.rotation
                                  * Quaternion.Euler(arm.rotationOffset + arm.rotationAdjustment);

        // Apply with smoothing during calibration
        float smoothAmount = isCalibrated ? arm.mimicAmount : Mathf.Clamp01(calibrationTimer / calibrationDuration);

        arm.armTarget.position = Vector3.Lerp(
            arm.armTarget.position,
            targetPosition,
            smoothAmount
        );

        arm.armTarget.rotation = Quaternion.Lerp(
            arm.armTarget.rotation,
            targetRotation,
            smoothAmount
        );
    }

    private void SetIKWeights()
    {
        if (leftArm.ikConstraint != null)
            leftArm.ikConstraint.weight = useIK ? leftArm.mimicAmount : 0f;
        if (rightArm.ikConstraint != null)
            rightArm.ikConstraint.weight = useIK ? rightArm.mimicAmount : 0f;
    }

    private void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        DrawArmGizmos(leftArm, Color.blue);
        DrawArmGizmos(rightArm, Color.red);
    }

    private void DrawArmGizmos(ArmSettings arm, Color color)
    {
        if (arm.vrController == null || arm.armTarget == null) return;

        Gizmos.color = color;
        Gizmos.DrawLine(arm.vrController.position, arm.armTarget.position);
        Gizmos.DrawSphere(arm.vrController.position, 0.02f);
        Gizmos.DrawSphere(arm.armTarget.position, 0.03f);
    }

    private void OnDisable()
    {
        // Reset to initial positions when disabled
        ResetArm(leftArm);
        ResetArm(rightArm);
    }

    private void ResetArm(ArmSettings arm)
    {
        if (arm.armTarget != null)
        {
            arm.armTarget.localPosition = arm.initialPosition;
            arm.armTarget.localRotation = arm.initialRotation;
        }
    }
}