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
    private float runSpeedModifier = 10f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float fallingModifier = 1;

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
    [HideInInspector] public bool isKnockbacking;

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
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .9f);
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
        if (!isKnockbacking)
        {
            float realSpeed;

            if (Input.GetButton("Run"))
                realSpeed = moveSpeed * runSpeedModifier;
            else
                realSpeed = moveSpeed;

            float horizontal = Input.GetAxis("Horizontal") * realSpeed;
            float vertical = Input.GetAxis("Vertical") * realSpeed;

            Vector3 movement = new Vector3(horizontal, rb.velocity.y, vertical);
            rb.velocity = transform.TransformVector(movement);
        }
    }

    private void Crouch()
	{

    }

	public void Knockback(Transform damageOrigin, float knockbackForce)
	{
        isKnockbacking = true;

        rb.velocity = Vector3.zero;
        rb.AddForce(damageOrigin.forward * knockbackForce, ForceMode.Impulse);
        rb.AddForce(transform.up * 4, ForceMode.Impulse);
    }

    public float GetNormalizedSpeed()
	{
        float normSpeed = rb.velocity.magnitude / moveSpeed;
        return normSpeed;
	}
}