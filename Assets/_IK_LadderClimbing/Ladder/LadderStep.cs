using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
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

    public void Awake()
    {
        BoxCollider myCollider = GetComponent<BoxCollider>();
        myCollider.isTrigger = true;
        myCollider.size = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
