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
}
