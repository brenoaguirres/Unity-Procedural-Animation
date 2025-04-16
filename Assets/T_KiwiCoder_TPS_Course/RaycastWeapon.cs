using NUnit.Framework;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    bool _isFiring = false;

    float _fireRate = 1.5f;
    float _fireTimer = 0f;

    bool _hasFiredAlready = false;

    ParticleSystem _muzzleFlash;
    AudioSource _fireSFX;

    [SerializeField] Transform _raycastOrigin;
    Ray _ray;
    RaycastHit _hitInfo;
    float _weaponRange = 10f;
    float _weaponDamage = 100f;

    private void Start()
    {
        _muzzleFlash = GetComponentInChildren<ParticleSystem>();
        _fireSFX = GetComponentInChildren<AudioSource>();

        Assert.IsNotNull(_muzzleFlash, "Muzzle flash not assigned in inspector.");    
        Assert.IsNotNull(_fireSFX, "Fire sound effect not assigned in inspector.");
    }

    private void Update()
    {
        FireRateUpdate();

        if (!_isFiring || _hasFiredAlready) return;

        FireSound();
        FireAnimation();
        Fire();
    }

    public void StartFiring()
    {
        _isFiring = true;
    }

    public void StopFiring()
    {
        _isFiring = false;
    }

    void FireRateUpdate()
    {
        if (_fireTimer <= 0)
        {
            ResetWeapon();
        }
        else if (_hasFiredAlready)
        {
            _fireTimer -= Time.deltaTime;
        }
    }
    
    void Fire()
    {
        _hasFiredAlready = true;

        _ray.origin = _raycastOrigin.position;
        _ray.direction = _raycastOrigin.forward;
        if (Physics.Raycast(_ray, out _hitInfo, _weaponRange))
        {
            IDamageable _damageable = _hitInfo.transform.GetComponent<IDamageable>();
            if (_damageable == null) return;

            _damageable.TakeDamage(_weaponDamage);
            Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * _weaponRange, Color.magenta, 1f);
        }
    }
    
    void ResetWeapon()
    {
        _fireTimer = _fireRate;
        _hasFiredAlready = false;
    }

    void FireAnimation()
    {
        _muzzleFlash.Emit(1);
    }

    void FireSound()
    {
        if (!_fireSFX.isPlaying)
            _fireSFX.PlayOneShot(_fireSFX.clip);
    }
}
