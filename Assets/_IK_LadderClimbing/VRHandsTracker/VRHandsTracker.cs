using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform VRTarget;
    public Transform rigTarget;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    public void Map()
    {
        if (rigTarget == null || VRTarget == null)
            return;
        rigTarget.position = VRTarget.TransformPoint(positionOffset);
        rigTarget.rotation = VRTarget.rotation * Quaternion.Euler(rotationOffset);
    }

    public void CalculateOffsets()
    {
        if (rigTarget == null || VRTarget == null)
            return;

        // Calcula o offset de posição no espaço local do VRTarget
        positionOffset = VRTarget.InverseTransformPoint(rigTarget.position);

        // Calcula o offset de rotação como a diferença em ângulos de Euler
        Quaternion rotationDifference = Quaternion.Inverse(VRTarget.rotation) * rigTarget.rotation;
        rotationOffset = rotationDifference.eulerAngles;
    }

}

public class VRHandsTracker : MonoBehaviour
{
    public VRMap rightHand;
    public VRMap leftHand;

    private void Start()
    {
        if (rightHand != null)
        {
            if (rightHand.VRTarget == null)
            {
                rightHand.VRTarget = GameObject.FindWithTag("VrRight")?.transform;
            }
            if (rightHand.VRTarget != null && rightHand.rigTarget != null)
            {
                rightHand.CalculateOffsets();
            }
        }

        if (leftHand != null)
        {
            if (leftHand.VRTarget == null)
            {
                leftHand.VRTarget = GameObject.FindWithTag("VrLeft")?.transform;
            }
            if (leftHand.VRTarget != null && leftHand.rigTarget != null)
            {
                leftHand.CalculateOffsets();
            }
        }
    }


    void Update()
    {
        rightHand.Map();
        leftHand.Map();
    }
}
