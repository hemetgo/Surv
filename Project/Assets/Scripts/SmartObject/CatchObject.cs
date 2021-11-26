using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObject : SmartObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Interact()
	{
        Debug.Log("Peguei");
        Destroy(gameObject);
	}
}
