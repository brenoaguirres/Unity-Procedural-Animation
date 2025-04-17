using UnityEngine;

public class LadderStep : MonoBehaviour
{
    public enum ELadderStepSide
    {
        R,
        L
    }

    public ELadderStepSide LadderStepSide;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }
}
