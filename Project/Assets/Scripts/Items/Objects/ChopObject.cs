using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChopObject : SmartObject
{
    [Header("Chop")]
    public ChopType chopType;
    public bool disableCollisionAfter;
    public ToolData.ToolType requiredTool;
    public int health;
    public bool damageRecovery;
    public bool destroyParent;
    public float destroyAfter;
    public int currentDamage;

    private float damageTimer;

    [Header("Drop")]
    public List<DropData> drops;

    [Header("Randomize")]
    public bool randomize;
    public float maxScaleVariation;

    public enum ChopType { EachInteract, WhenFinished }

	private void Start()
	{
        if (randomize) Randomize();
	}

	private void Update()
	{
		if (damageRecovery)
		{
            damageTimer += Time.deltaTime;
            if (damageTimer > 3) currentDamage = 0;
		}
	}

	public void Interact(int toolPower)
    {
        damageTimer = 0;
        switch (chopType)
        {
            case ChopType.EachInteract:
                if (health > 0)
                {
                    currentDamage += toolPower;
                    Drop();
                    if (currentDamage >= health)
                    {
                        if (!GetComponent<Rigidbody>())
                        {
                            Down();
                        }
                        if (destroyParent)
						{
                            Destroy(transform.parent.gameObject);
						}
						else
						{
                            Destroy(gameObject, destroyAfter);
                        }
                    }
                }
                break;
            case ChopType.WhenFinished:
                if (currentDamage < health)
                {
                    currentDamage += toolPower;
                    if (currentDamage >= health)
                    {
                        Drop();

                        if (!GetComponent<Rigidbody>())
                        {
                            Down();
                        }
                        Destroy(gameObject, destroyAfter);
                    }
                }
                break;
        }
    }

    private void Down()
	{
        Transform player = FindObjectOfType<FirstPersonController>().transform;
        if (disableCollisionAfter)
        {
            foreach(Collider col in GetComponents<Collider>())
			{
                Physics.IgnoreCollision(col, player.GetComponent<Collider>());
            }
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(col, player.GetComponent<Collider>());
            }
        }
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationY;
        rb.AddForceAtPosition(
            player.forward * 2,
            transform.position + GetComponent<Collider>().bounds.max, ForceMode.Impulse);
    }

    private void Drop()
	{
        List<DropData> dropps = new List<DropData>();
        foreach (DropData drop in drops)
		{
            if (Toolkit.RandomBool(drop.dropChance/100))
			{
                dropps.Add(drop);
			}
		}

        foreach (DropData drop in dropps)
        {
            for (int i = 0; i < drop.amount; i++)
            {
                SingleDrop(drop.itemData.drop);
            }
        }
    }

    private void SingleDrop(GameObject dropPrefab)
    {
        float height = GetComponent<Renderer>().bounds.size.y;
        GameObject drop = Instantiate(dropPrefab, transform.position + new Vector3(0, 
            Random.Range(0, height/2), 0), new Quaternion());
        Physics.IgnoreCollision(GetComponent<Collider>(), drop.GetComponent<Collider>());
        drop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), 2, Random.Range(-2, 2)), ForceMode.Impulse);
        drop.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
    }

    public override bool CanInteract(GameObject obj)
	{
        switch (chopType)
		{
            case ChopType.EachInteract:
                if (health > 0)
                {
                    ToolData tool = obj.GetComponent<HandManager>().handItem.itemData as ToolData;
                    if (tool != null)
                    {
                        if (tool.toolType == requiredTool)
                        {
                            return true;
                        }
                        else return false;
                    }
					else
					{
                        return false;
					}
                }
                else return false;

            case ChopType.WhenFinished:
                if (currentDamage < health)
                {
                    ToolData tool = obj.GetComponent<HandManager>().handItem.itemData as ToolData;
                    if (tool)
                    {
                        if (tool.toolType == requiredTool)
                        {
                            return true;
                        }
                        else return false;
                    }
                    else
					{
                        return false;
					}
                }
                else return false;

            default:
                return false;
        }
        
	}

    public override ObjectType GetObjectType()
    {
        return ObjectType.Chop;
    }

    public override string GetInteractButton()
    {
        return "Fire1";
    }

	private void Reset()
	{
        //if (!GetComponent<SavableObject>()) gameObject.AddComponent<SavableObject>();
	}

    public void SelfDrop()
	{
        chopType = ChopType.WhenFinished;
        damageRecovery = true;
        health = 3;
        requiredTool = ToolData.ToolType.Axe;

        ItemData itemData = Resources.Load<ItemData>("ItemData/Decoration/" + gameObject.name);
        DropData drop = new DropData();
        drop.itemData = itemData;
        drop.amountRange = Vector2Int.one;
        drop.dropChance = 100;
        
        drops = new List<DropData>();
        drops.Add(drop);
    }

    public void Randomize()
	{
        float variation = Random.Range(-maxScaleVariation, maxScaleVariation);
        transform.Rotate(0, Random.Range(0, 360), 0);
        transform.localScale = transform.localScale * (1 + variation);
  //      health = health * (1 + (int)variation);

  //      if (chopType == ChopType.WhenFinished)
		//{
  //          foreach (DropData drop in drops)
  //          {
		//        drop.amount = Random.Range(drop.amountRange.x, drop.amountRange.y);
  //              drop.amount = drop.amount * (int)(1 + variation);
  //          }
  //      }

        foreach(DropData drop in drops)
		{
            drop.amount = Random.Range(drop.amountRange.x, drop.amountRange.y);
        }
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChopObject))]
public class ChopObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChopObject script = (ChopObject)target;
        if (GUILayout.Button("Set Self Drop"))
        {
            script.SelfDrop();
        }
    }
}
#endif