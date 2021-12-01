using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    private float jumpForce = 5f;

    [Header("Camera Settings")]
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private GameObject hand;

    [Header("Input Settings")]
    [SerializeField]
    [Range(0.1f, 10f)]private float mouseSensibility = 5f;


    // Aux
    private Rigidbody rb;

    private float verticalAngle;
    private float horizontalAngle;
    private float height;
    private bool isCrouching;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        height = GetComponent<CapsuleCollider>().height;

        verticalAngle = 0f;
        horizontalAngle = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Jump();
            Camera();
        }
    }

    private void FixedUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Movement();
            Crouch();
        }
    }


    private void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.15f))
		{
            if (hit.collider) return true;
            else return false;
        } else
		{
            return false;
		}
    }

    private void Camera()
    {
        CameraX();
        CameraY();
    }

    private void CameraY()
    {
        var rotateCamera = -Input.GetAxis("Mouse Y");
        rotateCamera *= mouseSensibility;
        verticalAngle = Mathf.Clamp(rotateCamera + verticalAngle, -89f, 89f);
        var currentAngles = camPosition.transform.localEulerAngles;
        currentAngles.x = verticalAngle;
        camPosition.transform.localEulerAngles = currentAngles;
    }

    private void CameraX()
    {
        var rotatePlayer = Input.GetAxis("Mouse X") * mouseSensibility;
        horizontalAngle += rotatePlayer;

        if (horizontalAngle > 360)
            horizontalAngle -= 360f;
        if (horizontalAngle < 0)
            horizontalAngle += 360f;

        var currentAngles = transform.localEulerAngles;
        currentAngles.y = horizontalAngle;
        transform.localEulerAngles = currentAngles;
    }

    private void Movement()
    {
        float realSpeed;

        if (Input.GetButton("Run"))
            realSpeed = runSpeed;
        else
            realSpeed = moveSpeed;

        float horizontal = Input.GetAxis("Horizontal") * realSpeed; 
        float vertical = Input.GetAxis("Vertical") * realSpeed;

        Vector3 movement = new Vector3(horizontal, rb.velocity.y, vertical);
        rb.velocity = transform.TransformVector(movement);
    }

    private void Crouch()
	{

    }
}