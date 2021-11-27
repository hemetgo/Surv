using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopObject : SmartObject
{
    public Item.ToolType requiredTool;
    public GameObject dropPrefab;
    public int stock;

    public override void Interact()
    {
        if (stock > 0) {
            stock -= 1;
            GameObject drop = Instantiate(dropPrefab, transform.position + new Vector3(0, 2, 0), new Quaternion());
            Physics.IgnoreCollision(GetComponent<Collider>(), drop.GetComponent<Collider>());
            drop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), 2, Random.Range(-2, 2)), ForceMode.Impulse);
            drop.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            
            if (stock <= 0)
			{
                if (!GetComponent<Rigidbody>())
                {
                    gameObject.AddComponent<Rigidbody>();
                    Destroy(gameObject, 5);
                }
            }
        }
    }

    public override bool CanInteract(GameObject obj)
	{
        if (stock > 0) 
        {
            if (obj.GetComponent<HandManager>().handItemData.itemName.Contains(requiredTool.ToString()))
            {
                return true;
			} 
            else return false;
        } 
        else return false;
	}

    public override ObjectType GetObjectType()
    {
        return ObjectType.Chop;
    }
}
