using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject : MonoBehaviour
{
    public GameObject particle;

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

}
