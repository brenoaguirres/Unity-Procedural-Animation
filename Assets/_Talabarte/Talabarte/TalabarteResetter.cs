using UnityEngine;

public class TalabarteResetter : MonoBehaviour
{
    public Talabarte talabarte1;
    public Talabarte talabarte2;
    public Transform beltTransform;

    private void OnTriggerEnter(Collider other)
    {
        Talabarte t = other.GetComponent<Talabarte>();
        if (t != null)
        {
            t.ResetToBelt(beltTransform);
        }
    }
}
