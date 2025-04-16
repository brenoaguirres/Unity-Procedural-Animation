using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    Camera _mainCamera;

    Ray _ray;
    RaycastHit _hitInfo;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        _ray.origin = _mainCamera.transform.position;
        _ray.direction = _mainCamera.transform.forward;
        Physics.Raycast(_ray, out _hitInfo);
        transform.position = _hitInfo.point;
    }
}
