using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMobController : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public Material dmgMaterial;
    public GameObject deathParticles;

    [Header("GUI")]
    public Image healthBar;
    public Image healthBarBackground;

    [Header("Drops")]
	public List<DropData> dropsData;

    private Material originalMaterial;
    private Rigidbody rb;
    private AiAgent aiAgent;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.gameObject.SetActive(false);
        healthBarBackground.gameObject.SetActive(false);
        currentHp = maxHp;
        rb = GetComponent<Rigidbody>();
        originalMaterial = GetComponentInChildren<Renderer>().material;
        aiAgent = GetComponent<AiAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthToPlayer();
    }

    public void HealthToPlayer()
	{
        healthBar.transform.LookAt(aiAgent.player);
        healthBarBackground.transform.LookAt(aiAgent.player);
	}

    public void TakeDamage(int dmg, Transform damageOrigin, float knockbackForce)
	{
        if (currentHp > 0)
        {
            currentHp -= dmg;
            rb.velocity = Vector3.zero;
            rb.AddForce(damageOrigin.forward * knockbackForce, ForceMode.Impulse);
            rb.AddForce(transform.up * 4, ForceMode.Impulse);
            GetComponentInChildren<Renderer>().material = dmgMaterial;
            Invoke("ResetMaterial", 0.05f);

            healthBar.fillAmount = (float)currentHp / (float)maxHp;
            healthBar.gameObject.SetActive(true);
            healthBarBackground.gameObject.SetActive(true);
        }

        if (currentHp <= 0)
		{
            Physics.IgnoreCollision(GetComponent<Collider>(), aiAgent.player.GetComponent<Collider>());
            healthBar.gameObject.SetActive(false);
            healthBarBackground.gameObject.SetActive(false);
            aiAgent.animator.SetTrigger("Die");
            Invoke("Die", 0.6f);
		} else
		{
            aiAgent.animator.SetTrigger("Take Damage");
        }
    }

    private void Die()
	{
        Instantiate(deathParticles, transform.position - (transform.up * GetComponent<CapsuleCollider>().center.y), transform.rotation);
        List<DropData> drops = new List<DropData>();
        foreach (DropData drop in dropsData)
		{
            if (Toolkit.RandomBool(drop.dropChance/100))
			{
                drops.Add(drop);
			}
		}

        foreach(DropData drop in drops)
		{
            for (int i = 0; i < drop.amount; i++)
			{
                Drop(drop.itemData.drop);
            }
		}

        Destroy(gameObject);
    }

    private void Drop(GameObject dropPrefab)
    {
        GameObject drop = Instantiate(dropPrefab, transform.position + new Vector3(0, 2, 0), new Quaternion());
        Physics.IgnoreCollision(GetComponent<Collider>(), drop.GetComponent<Collider>());
        drop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), 2, Random.Range(-2, 2)), ForceMode.Impulse);
        drop.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
    }

    private void ResetMaterial()
    {
        GetComponentInChildren<Renderer>().material = originalMaterial;
    }

    public int GetCurrentHp()
	{
        return currentHp;
	}
}
