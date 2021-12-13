using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject : MonoBehaviour
{
    public GameObject particle;
    
    public enum ObjectType { None, Chop, Furniture }

    public virtual void Interact()
    {
    }

    public virtual void Interact(GameObject obj)
    {
    }

    public virtual bool CanInteract(GameObject obj)
	{
        return false;
    }

    public virtual ObjectType GetObjectType()
    {
        return ObjectType.None;
    }
}
