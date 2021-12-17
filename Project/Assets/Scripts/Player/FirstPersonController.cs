using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstPersonController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    [Tooltip("Multiplicate the movespeed")] public float runSpeedModifier = 10f;
    public float maxStamina;
    public float staminaModifier;
    public float jumpForce = 5f;
    [Tooltip("Divide the movespeed")] public float crouchSpeedModifier = 2f;
    public float fallDmgModifier = 1;
    public float speedLimitFallDamage = 7;
    [HideInInspector] public float currentStamina;
    public bool isEating;

    [Header("Camera Settings")]
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private GameObject head;

    [Header("Input Settings")]
    [SerializeField]
    [Range(0.1f, 10f)]private float mouseSensibility = 5f;


    // Aux
    private Rigidbody rb;
    [HideInInspector] public bool isKnockbacking;

    private float verticalAngle;
    private float horizontalAngle;
    private float capsuleHeight;
    private float capsuleCenter;
    private CapsuleCollider capsule;
    private bool isCrouching;
    private bool isRunning;
    private float fallingDamage;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        capsuleHeight = capsule.height;
        capsuleCenter = capsule.center.y;

        verticalAngle = 0f;
        horizontalAngle = transform.localEulerAngles.y;

        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Jump();
            Camera();
        }

        StaminaControl();
        FallDamage();
    }

    private void FixedUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Movement();
            Crouch();
        }

        if (Singleton.Instance.OnMenu())
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            isRunning = false;
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0); // Para evitar que o movimento trave e o plauer se movimente sozinho

        if (!isKnockbacking)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            float realSpeed;

            if (Input.GetButton("Run") && (Input.GetKey(KeyCode.W) 
                || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) 
                || Input.GetKey(KeyCode.D)) && currentStamina > 0)
            {
                realSpeed = moveSpeed * runSpeedModifier;
                isRunning = true;

                //if (Mathf.Abs(horizontal) > moveSpeed / 8 || Mathf.Abs(vertical) > moveSpeed / 8) isRunning = true;
                //else isRunning = false;
            }
            else
            {
                realSpeed = moveSpeed;
                isRunning = false;
            }

            if (isCrouching || isEating) realSpeed = realSpeed / crouchSpeedModifier;
            if (vertical < 0) realSpeed = realSpeed / 2;

            Vector3 moveInput = new Vector3(horizontal, 0, vertical);
            Vector3 moveVector = transform.TransformDirection(moveInput) * realSpeed;
            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
        }
    }

    private void StaminaControl()
	{
        if (isRunning)
        {
            currentStamina -= Time.deltaTime * staminaModifier;
        }
        else
        {
            currentStamina += Time.deltaTime * staminaModifier / 2;
        }
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FallDamage()
	{
        if (rb.velocity.y < 0)
		{
            bool trigger = false;
            if (rb.velocity.y < -speedLimitFallDamage)
			{
                trigger = true;
                fallingDamage += Time.deltaTime * fallDmgModifier;
            }

            if (IsGrounded())
			{
                if (trigger) head.GetComponent<Animator>().SetTrigger("Shake");
                GetComponent<HealthController>().TakeDamage((int)fallingDamage, transform, 0);
                fallingDamage = 0;
            }
        }
	}

    private bool IsGrounded()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * .9f);
    }

    private bool IsKnockingHead()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.max.y, col.bounds.center.z), col.radius * .9f);
    }

    private void Camera()
    {
        head.transform.position = new Vector3(head.transform.position.x, capsule.bounds.max.y - capsuleCenter/8, head.transform.position.z);

        //CAMERA X
        var rotatePlayer = Input.GetAxis("Mouse X") * mouseSensibility;
        horizontalAngle += rotatePlayer;

        if (horizontalAngle > 360)
            horizontalAngle -= 360f;
        if (horizontalAngle < 0)
            horizontalAngle += 360f;

        var currentAnglesX = transform.localEulerAngles;
        currentAnglesX.y = horizontalAngle;
        transform.localEulerAngles = currentAnglesX;

        // CAMERA Y
        var rotateCamera = -Input.GetAxis("Mouse Y");
        rotateCamera *= mouseSensibility;
        verticalAngle = Mathf.Clamp(rotateCamera + verticalAngle, -89f, 89f);
        var currentAnglesY = camPosition.transform.localEulerAngles;
        currentAnglesY.x = verticalAngle;
        camPosition.transform.localEulerAngles = currentAnglesY;
    }

    private void Crouch()
	{
        if (Input.GetButton("Crouch"))
        {
            isCrouching = true;
            capsule.height = Mathf.Lerp(capsule.height, capsuleHeight / 2.5f, 0.3f);
            capsule.center = new Vector3(0, Mathf.Lerp(capsule.center.y, capsuleCenter / 2.5f, 0.3f), 0);
        }
        else
        {
            if (!IsKnockingHead())
            {
                isCrouching = false;
                capsule.height = Mathf.Lerp(capsule.height, capsuleHeight, 0.3f);
                capsule.center = new Vector3(0, Mathf.Lerp(capsule.center.y, capsuleCenter, 0.3f), 0);
            }
        }
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