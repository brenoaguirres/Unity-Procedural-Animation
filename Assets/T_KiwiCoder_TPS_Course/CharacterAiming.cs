using NUnit.Framework;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float _turnSpeed = 10f;
    Camera _mainCamera;

    [SerializeField] Rig _aimLayer;
    [SerializeField] Rig _bodyAimLayer;
    [SerializeField] GameObject _laserSight;
    float _aimDuration = 0.25f;

    bool _isAiming = false;

    RaycastWeapon _raycastWeapon;

    void Start()
    {
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _raycastWeapon = GetComponentInChildren<RaycastWeapon>();

        Assert.IsNotNull(_aimLayer, "Aim layer not assigned in inspector.");
        Assert.IsNotNull(_bodyAimLayer, "Body aim layer not assigned in inspector.");
        Assert.IsNotNull(_laserSight, "Laser sight not assigned in inspector.");
        Assert.IsNotNull(_raycastWeapon, "Raycast weapon not assigned in inspector.");
    }

    private void Update()
    {
        Aim();
        Fire();
    }

    void FixedUpdate()
    {
        float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), _turnSpeed * Time.fixedDeltaTime);
    }

    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            _aimLayer.weight = Mathf.Clamp01(_aimLayer.weight + (Time.deltaTime / _aimDuration));
            _bodyAimLayer.weight = Mathf.Clamp01(_aimLayer.weight + (Time.deltaTime / _aimDuration));
            if (!_laserSight.activeSelf) _laserSight.SetActive(true);
            _isAiming = true;

            return;
        }

        _aimLayer.weight = Mathf.Clamp01(_aimLayer.weight - (Time.deltaTime / _aimDuration));
        _bodyAimLayer.weight = Mathf.Clamp01(_aimLayer.weight - (Time.deltaTime / _aimDuration));
        if (_laserSight.activeSelf) _laserSight.SetActive(false);
        _isAiming = false;
    }

    void Fire()
    {
        if (!_isAiming)
        {
            _raycastWeapon.StopFiring();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _raycastWeapon.StartFiring();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _raycastWeapon.StopFiring();
        }
    }
}
