using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;

    [Header("Properties")]
    public float moveSpeed;
    public float jumpForce;
    public float viewRange;
    public float chaseRangeModifier;

    [Header("Battle")]
    public int damagePower;
    public float knockbackPower;

    [HideInInspector] public Transform player;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiPatrolState());
        stateMachine.RegisterState(new AiChaseState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stateMachine.Update();
    }

    public bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.15f))
        {
            if (hit.collider) return true;
            else return false;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerOnView(float viewModifier)
	{
        if (Vector3.Distance(transform.position, player.position) <= viewRange * viewModifier)
            return true;
        else
            return false;    
	}

    public bool IsFrontHitting()
	{
        Vector3 rayOrigin = transform.position;
        Vector3 rayDir = transform.forward;
        float rayDist = GetComponent<CapsuleCollider>().height / 2 + 0.5f;
        if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hit, rayDist))
        {
            if (hit.collider)
            {
                return true;
            }
        }

        return false;
    }

    public void Jump()
	{
        animator.SetTrigger("Jump");
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


	private void OnDrawGizmosSelected()
	{
        Color gizmosColor = Color.yellow;
        gizmosColor.a = 0.1f;
        Gizmos.color = gizmosColor;

        Gizmos.DrawSphere(transform.position, viewRange);
    }
}
