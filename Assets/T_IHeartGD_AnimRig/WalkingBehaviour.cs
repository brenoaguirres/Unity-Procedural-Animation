using UnityEngine;

public class WalkingBehaviour : MonoBehaviour
{
    private Vector2 input = new();
    private Rigidbody rb;
    private Animator anim;
    private float ms = 3f;

    private Quaternion targetRotation;
    private float rs = 0.15f;
    private float rotFactor = 0.0f;
    private float angleThreshold = 1f;

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

        ApplyRotation();
        SetTargetRotation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(input.x * ms * -1, rb.linearVelocity.y, input.y * ms * -1);
    }
    private void SetTargetRotation()
    {
        targetRotation = Quaternion.Euler(0, Mathf.Atan2(-input.x, -input.y) * Mathf.Rad2Deg, 0);

        if (Quaternion.Angle(transform.rotation, targetRotation) > angleThreshold) rotFactor = 0.0f;
    }
    private void ApplyRotation()
    {
        if (targetRotation == null || rotFactor >= 1.0f) return;

        rotFactor += Time.deltaTime / rs;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotFactor);
    }
}
