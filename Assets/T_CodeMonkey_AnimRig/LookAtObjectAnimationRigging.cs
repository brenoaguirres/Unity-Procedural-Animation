using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtObjectAnimationRigging : MonoBehaviour
{
    private Rig _headRig;
    private Transform _headBone;
    private BoxCollider _headCollider;
    private Transform _targetTransform;
    private Vector3 _defaultTargetTransformPosition;
    private LayerMask _layer;
    private Transform _targetObject;

    private void Start()
    {
        _layer = LayerMask.GetMask("Interactable");

        _headRig = GetComponent<Rig>();
        _headRig.weight = 0f;

        _headCollider = GetComponent<BoxCollider>();
        _headCollider.isTrigger = true;

        _targetTransform = GetComponentInChildren<MultiAimConstraint>().data.sourceObjects[0].transform;
        _headBone = GetComponentInChildren<MultiAimConstraint>().data.constrainedObject.transform;
        _defaultTargetTransformPosition = _targetTransform.localPosition;
    }

    private void Update()
    {
        if (_targetObject == null) return;

        LookAtObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targetObject != null || other.gameObject.layer != LayerMask.NameToLayer("Interactable")) return;

        _targetObject = other.transform;
        _targetTransform.position = _targetObject.position;
        _headRig.weight = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != _targetObject) return;

        ResetLookAt();
    }

    private void LookAtObject()
    {
        _targetTransform.position = _targetObject.position;

        if (Vector3.Dot(_headBone.position.normalized, _targetTransform.position.normalized) < 0.1f)
        {
            ResetLookAt();
        }
    }

    private void ResetLookAt()
    {
        _targetObject = null;
        _headRig.weight = 0f;
        _targetTransform.localPosition = _defaultTargetTransformPosition;
    }
}
