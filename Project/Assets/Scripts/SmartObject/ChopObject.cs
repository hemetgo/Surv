using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopObject : SmartObject
{
    public ChopType chopType;
    public ItemData.ToolType requiredTool;
    public GameObject dropPrefab;
    public int stock;
    private int damage;

    public enum ChopType { EachInteract, WhenFinished }
    public override void Interact()
    {
		switch (chopType)
		{
            case ChopType.EachInteract:
                if (stock > 0)
                {
                    stock -= 1;
                    Drop();

                    if (stock <= 0)
                    {
                        if (!GetComponent<Rigidbody>())
                        {
                            gameObject.AddComponent<Rigidbody>();
                            Destroy(gameObject, 5);
                        }
                    }
                }
                break;
            case ChopType.WhenFinished:
                if (damage < stock)
                {
                    damage += 1;
                    if (damage == stock)
                    {
                        for (int i = 0; i < stock; i++)
                        {
                            Drop();
                        }

                        if (!GetComponent<Rigidbody>())
                        {
                            gameObject.AddComponent<Rigidbody>();
                            Destroy(gameObject, 5);
                        }
                    }
                }
                break;
        }
    }

    private void Drop()
	{
        GameObject drop = Instantiate(dropPrefab, transform.position + new Vector3(0, 2, 0), new Quaternion());
        Physics.IgnoreCollision(GetComponent<Collider>(), drop.GetComponent<Collider>());
        drop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), 2, Random.Range(-2, 2)), ForceMode.Impulse);
        drop.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
    }

    public override bool CanInteract(GameObject obj)
	{
        switch (chopType)
		{
            case ChopType.EachInteract:
                if (stock > 0)
                {
                    if (obj.GetComponent<HandManager>().handItemData.itemData.itemName.english.Contains(requiredTool.ToString()))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            case ChopType.WhenFinished:
                if (damage < stock)
                    if (obj.GetComponent<HandManager>().handItemData.itemData.itemName.english.Contains(requiredTool.ToString()))
                    {
                        return true;
                    }
                    else return false;
                else return false;
            default:
                return false;
        }
        
	}

    public override ObjectType GetObjectType()
    {
        return ObjectType.Chop;
    }
}
