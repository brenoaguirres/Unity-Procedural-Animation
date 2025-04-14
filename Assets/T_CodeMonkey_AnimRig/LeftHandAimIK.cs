using UnityEngine;

public class LeftHandAimIK : MonoBehaviour
{
    public enum ELeftHandAimIKState
    {
        Idle,
    }

    public ELeftHandAimIKState StateKey = ELeftHandAimIKState.Idle;

    public Transform Target;
    public Transform AimPosition;

    public void Update()
    {
        if (StateKey == ELeftHandAimIKState.Idle)
        {
            Target.position = AimPosition.position;
        }
    }
}
