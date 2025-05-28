/*
using UnityEngine;
using Meta.XR.BuildingBlocks;

public class Talabarte : MonoBehaviour
{
    private MetaGrabbable grabbable;
    private Rigidbody rb;
    private BoxCollider boxCollider;

    public bool attached = false;
    public Transform currentParent;

    private void Awake()
    {
        // Find the grabbable component in the child
        grabbable = GetComponentInChildren<MetaGrabbable>();

        if (grabbable == null)
        {
            Debug.LogError("No MetaGrabbable found in child objects.");
            return;
        }

        rb = grabbable.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on MetaGrabbable object.");
            return;
        }

        boxCollider = grabbable.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider found on MetaGrabbable object.");
            return;
        }

        // Subscribe to grab events
        grabbable.OnGrabStarted.AddListener(OnGrab);
        grabbable.OnGrabEnded.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        if (grabbable != null)
        {
            grabbable.OnGrabStarted.RemoveListener(OnGrab);
            grabbable.OnGrabEnded.RemoveListener(OnRelease);
        }
    }

    private void Update()
    {
        if (attached && currentParent != null)
        {
            grabbable.transform.localPosition = Vector3.zero;
            grabbable.transform.localRotation = Quaternion.identity;
        }
    }

    private void OnGrab()
    {
        DisablePhysics();
    }

    private void OnRelease()
    {
        EnablePhysics();
    }

    private void DisablePhysics()
    {
        if (boxCollider != null) boxCollider.isTrigger = true;
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void EnablePhysics()
    {
        if (boxCollider != null) boxCollider.isTrigger = false;
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
*/