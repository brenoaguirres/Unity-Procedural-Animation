using UnityEngine;

public class TalabarteRepositioner : MonoBehaviour
{
    // References to Talabarte objects
    public GameObject Talabarte1;
    public GameObject Talabarte2;

    // Default positions
    private Vector3 Talabarte1Position;
    private Vector3 Talabarte2Position;

    private void Start()
    {
        if (Talabarte1 != null)
        {
            Talabarte1Position = Talabarte1.transform.position;
        }

        if (Talabarte2 != null)
        {
            Talabarte2Position = Talabarte2.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Talabarte1)
        {
            Talabarte1.transform.position = Talabarte1Position;
            Debug.Log("Talabarte1 reset to default position.");
        }
        else if (other.gameObject == Talabarte2)
        {
            Talabarte2.transform.position = Talabarte2Position;
            Debug.Log("Talabarte2 reset to default position.");
        }
    }
}
