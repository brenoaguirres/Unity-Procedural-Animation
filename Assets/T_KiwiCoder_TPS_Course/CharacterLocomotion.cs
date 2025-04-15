using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator _animator;
    Rigidbody _rigidbody;
    Vector2 _input;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        _animator.SetFloat("InputX", _input.x);
        _animator.SetFloat("InputY", _input.y);
    }
    /*
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(_input.x, 0, _input.y) * 5f;
        movement.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = movement;


        Vector3 gravity = -9.81f * 10 * Vector3.up;
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
    */
}
