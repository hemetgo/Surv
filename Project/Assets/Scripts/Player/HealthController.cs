using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    private InteractController interactController;
    private Rigidbody rb;
    private Transform head;
    private HandManager handManager;
    private float range;
    private float dmgTimer;
    private float atkDelay;
    private float atkTimer;

    public delegate void UpdatedHealthHandler(int hp);
    public event UpdatedHealthHandler UpdatedHealth;

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
                        handManager.handItem.RemoveDurability(handManager);
                        damageTaker.TakeDamage(GetBattlePower().damage, transform, GetBattlePower().knockBack);
                    }
                }
            }
        }
    }

    public void TakeDamage(int dmg, Transform damageOrigin, float knockbackForce)
    {
        currentHp -= dmg;
        if (knockbackForce > 0)
            GetComponent<FirstPersonController>().Knockback(damageOrigin, knockbackForce);

        UpdatedHealth(currentHp);

        if (currentHp <= 0)
        {
            GetComponent<FirstPersonController>().enabled = false;
        }
    }

    public BattlePower GetBattlePower()
    {
        BattlePower power = new BattlePower();
        if (handManager.handItem.itemData)
        {
            ItemData item = handManager.handItem.itemData;
            power.damage = item.power;
            power.knockBack = item.knockbackForce;

            if (Toolkit.RandomBool(item.critRate / 100))
			{
                power.damage = power.damage * 2;
                power.knockBack = power.knockBack * 2;
            }
        }
        
        return power;
    }

    public void Heal(int healPower)
	{
        currentHp += healPower;
        if (currentHp > maxHp) currentHp = maxHp;
        UpdatedHealth(currentHp);
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
