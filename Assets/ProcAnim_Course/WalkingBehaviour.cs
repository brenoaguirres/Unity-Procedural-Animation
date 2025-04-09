using UnityEngine;

public class WalkingBehaviour : MonoBehaviour
{
    private Vector2 input = new();
    private Rigidbody rb;
    private Animator anim;
    private float ms = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        if (input != Vector2.zero)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(input.x * ms, rb.linearVelocity.y, input.y * ms);
    }
}
