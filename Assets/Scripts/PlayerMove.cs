using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float acceleration = 10f;     
    public bool rotateTowardsMoveDirection = true;
    public float rotationSpeed = 10f;
    private Rigidbody rb;
    private Vector3 inputDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {

        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.W))
        {

            v -= 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            v += 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            h -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {

            h += 1f;
        }

        inputDir = new Vector3(h, 0f, v);
        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();
        
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = inputDir * moveSpeed;
        targetVelocity.y = rb.velocity.y; 

        Vector3 velocityChange = targetVelocity - rb.velocity;
        velocityChange.y = 0f;

        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);

        if (rotateTowardsMoveDirection && inputDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDir, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}