using UnityEngine;
using System;
using System.Collections;

public class Talabarte : MonoBehaviour
{
    public enum SnapState { Free, Hand, LadderSnap, Belt }

    public bool isDebug = true;
    public float cooldownSnap = 1f;

    public Action OnHandSnap;
    public Action OnBeltSnap;
    public Action OnLadderSnap;

    private bool canSnap = true;
    private float cooldownTimer = 0f;

    private SnapState currentSnapState = SnapState.Free;

    [SerializeField] private TalabarteManager talabarteManager;

    private void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void UpdateCooldown()
    {
        if (!canSnap)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canSnap = true;
                cooldownTimer = 0f;
            }
        }
    }

    public void EnablePhysics()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public void DisablePhysics()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TrySnap(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TrySnap(other);
    }

    private void TrySnap(Collider other)
    {
        if (!canSnap) return;

        SnapState state = SnapState.Free;
        switch (other.tag)
        {
            case "Hand":
                state = SnapState.Hand;
                break;
            case "Belt":
                state = SnapState.Belt;
                break;
            case "LadderSnap":
                state = SnapState.LadderSnap;
                break;
            default:
                return;
        }

        Snap(other.transform, state);
    }

    public void Snap(Transform target, SnapState newState)
    {
        if (currentSnapState == SnapState.LadderSnap) return;
        if (currentSnapState == newState) return;

        transform.SetParent(target);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        currentSnapState = newState;
        canSnap = false;
        cooldownTimer = cooldownSnap;

        switch (newState)
        {
            case SnapState.Hand:
                OnHandSnap?.Invoke();
                if (isDebug) Debug.Log("OnHandSnap");
                break;
            case SnapState.Belt:
                OnBeltSnap?.Invoke();
                if (isDebug) Debug.Log("OnBeltSnap");
                break;
            case SnapState.LadderSnap:
                OnLadderSnap?.Invoke();
                if (isDebug) Debug.Log("OnLadderSnap");
                talabarteManager?.OnTalabarteLadderSnapped(); // Notify manager
                break;
        }
    }

    public void ResetToBelt(Transform beltTransform)
    {
        Snap(beltTransform, SnapState.Belt);
    }

    public void UnlockLadderSnapped()
    {
        currentSnapState = SnapState.Free;
        canSnap = true;
    }
}
