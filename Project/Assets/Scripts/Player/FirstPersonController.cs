using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private GameObject hand;

    [Header("Input Settings")]
    [SerializeField]
    private bool useRigibody;
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
    [SerializeField]
    private float pushPower = 1.5f;

    private float verticalSpeed = 0f;
    private CharacterController controller;
    private Rigidbody rb;

    private float verticalAngle;
    private float horizontalAngle;
    private float height;
    private bool isCrouching;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (useRigibody)
        {
            rb = GetComponent<Rigidbody>();
            height = GetComponent<CapsuleCollider>().height;
        }
        else
        {
            controller = GetComponent<CharacterController>();
            height = controller.height;
        }

        verticalAngle = 0f;
        horizontalAngle = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            Jump();
            Camera();
            Gravity();
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            Movement();
            Crouch(false);
        }
    }


    private void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            if (useRigibody)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
            else
            {

                verticalSpeed = jumpForce;
            }
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


        if (useRigibody)
        {
            float horizontal = Input.GetAxis("Horizontal") * realSpeed; 
            float vertical = Input.GetAxis("Vertical") * realSpeed;

            Vector3 movement = new Vector3(horizontal, rb.velocity.y, vertical);
            rb.velocity = transform.TransformVector(movement);

            //float x = Input.GetAxis("Horizontal");
            //float z = Input.GetAxis("Vertical");

            //// Some sticks/arrow keys will let both axes be 1 at once.
            // ClampMagnitude ensures diagonals obey our max speed.
            //Vector3 localVelocity = Vector3.ClampMagnitude(new Vector3(x, rb.velocity.y, z), 1) * realSpeed;

            //rb.velocity = transform.TransformDirection(localVelocity);
        }
        else
        {
            var movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));            

            if (movementInput.sqrMagnitude > 1f)
                movementInput.Normalize();

            var movement = movementInput * realSpeed * Time.deltaTime;
            movement = transform.TransformDirection(movement);


            controller.Move(movement);
            hand.GetComponent<Animator>().SetFloat("MoveSpeed", controller.velocity.magnitude / moveSpeed);
        }
    }

    private void Crouch(bool canCrouch)
	{
        if (canCrouch)
        {
            if (useRigibody)
            {

            } 
            else
            {

                if (Input.GetButton("Crouch"))
                {
                    controller.height = height / 2;
                }
                else
                {
                    controller.height = height;
                }
            }
        }
    }

    private void Gravity()
    {
        if (!useRigibody)
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


    /*
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
*/
}