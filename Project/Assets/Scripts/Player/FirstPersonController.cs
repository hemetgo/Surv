using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private GameObject hand;

    [Header("Input Settings")]
    [SerializeField]
    [Range(0.1f, 10f)]private float mouseSensibility = 5f;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    private float jumpForce = 5f;

    [Header("World Settings")]
    [SerializeField]
    [Range(1, 25f)] private float gravity = 10f;

    private float verticalSpeed = 0f;
    private CharacterController controller;

    private float verticalAngle;
    private float horizontalAngle;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        verticalAngle = 0f;
        horizontalAngle = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            var isJumping = Input.GetButton("Jump");
            
            Jump(isJumping);
            Camera();
            Gravity();
        }
    }

	private void FixedUpdate()
	{
        var movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var isRunning = Input.GetButton("Run"); 
        Movement(movementInput, isRunning);
        Crouch();
    }


    private void Jump(bool isJumping)
    {
        if (IsGrounded() && isJumping)
        {
            verticalSpeed = jumpForce;
        }
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, (controller.height / 2) + 0.1f))
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

    private void Movement(Vector3 movementInput, bool isRunning)
    {
        float realSpeed;
        if (isRunning)
            realSpeed = runSpeed;
        else
            realSpeed = moveSpeed;

        if (movementInput.sqrMagnitude > 1f)
            movementInput.Normalize();

        var movement = movementInput * realSpeed * Time.deltaTime;
        movement = transform.TransformDirection(movement);
        controller.Move(movement);
        hand.GetComponent<Animator>().SetFloat("MoveSpeed", controller.velocity.magnitude / moveSpeed);
    }

    private void Crouch()
	{
        if (Input.GetButton("Crouch"))
		{
            controller.height = 1;
        }
        else
		{
            controller.height = 2;
        }
    }

    private void Gravity()
    {
        this.verticalSpeed = this.verticalSpeed - gravity * Time.deltaTime;
        if (this.verticalSpeed < -gravity)
        {
            this.verticalSpeed = -gravity; // velocidade maxima de queda
        }
        var verticalSpeed = new Vector3(0, this.verticalSpeed * Time.deltaTime, 0);
        var flagCollision = controller.Move(verticalSpeed);
        if ((flagCollision & CollisionFlags.Below) != 0)
        {
            this.verticalSpeed = 0;
        }
    }
}