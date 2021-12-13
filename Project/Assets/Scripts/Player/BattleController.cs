using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public int maxHp;
    [HideInInspector] public int currentHp;

    private InteractController interactController;
    private Rigidbody rb;
    private Transform head;
    private HandManager handManager;
    private float range;
    private float dmgTimer;
    private float atkDelay;
    private float atkTimer;

    public delegate void TakenDamageHandler(int hp);
    public event TakenDamageHandler TakenDamage;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        interactController = GetComponent<InteractController>();
        rb = GetComponent<Rigidbody>();
        head = interactController.head;
        range = interactController.interactRange;
        handManager = FindObjectOfType<HandManager>();
        atkDelay = interactController.actionDelay;
    }

    // Update is called once per frame
    void Update()
    {
        dmgTimer += Time.deltaTime;
        atkTimer += Time.deltaTime;

        if (atkTimer >= atkDelay)
        {
            if (Physics.Raycast(head.transform.position, head.transform.forward, out RaycastHit hit, range))
            {
                if (hit.collider.gameObject.GetComponent<BattleMobController>())
                {
                    BattleMobController damageTaker = hit.collider.gameObject.GetComponent<BattleMobController>();
                    if (Input.GetButtonDown("Fire1"))
                    {
                        atkTimer = 0;
                        damageTaker.TakeDamage(GetDamagePower(), transform, GetKnockbackForce());
                    }
                }
            }
        }
    }

    public void TakeDamage(int dmg, Transform damageOrigin, float knockbackForce)
    {
        currentHp -= dmg;
        GetComponent<FirstPersonController>().Knockback(damageOrigin, knockbackForce);
        TakenDamage(currentHp);

        if (currentHp <= 0)
        {
            GetComponent<FirstPersonController>().enabled = false;
        }
    }

    public int GetDamagePower()
    {
        if (handManager.handItem.itemData)
        {
            return handManager.handItem.itemData.power;
        }
        else return 1;
    }

    public int GetKnockbackForce()
    {
        if (handManager.handItem.itemData)
        {
            return handManager.handItem.itemData.knockbackForce;
        }
        else return 10;
    }


    private void OnCollisionStay(Collision collision)
	{
        if (collision.gameObject.GetComponent<AiAgent>())
        {
            if (collision.gameObject.GetComponent<BattleMobController>().GetCurrentHp() > 0)
            {
                if (dmgTimer >= 0.5f)
                {
                    dmgTimer = 0;
                    AiAgent agent = collision.gameObject.GetComponent<AiAgent>();
                    TakeDamage(agent.damagePower, agent.transform, agent.knockbackPower);
                }
            }
        } else
		{
            GetComponent<FirstPersonController>().isKnockbacking = false;
        }
    }

}
