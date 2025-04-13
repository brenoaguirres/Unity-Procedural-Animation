using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LegsIKAnimationRigging : MonoBehaviour
{
    private Rigidbody _rb;
    private Rig _legsRig;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();

        _legsRig = GetComponent<Rig>();
    }

    private void Update()
    {
        if (_rb == null) return;

        if (_rb.linearVelocity == Vector3.zero)
        {
            _legsRig.weight = 1;
        }
        else
        {
            _legsRig.weight = 0;
        }
    }
}
