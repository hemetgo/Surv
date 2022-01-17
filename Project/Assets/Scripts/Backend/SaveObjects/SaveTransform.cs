using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveTransform : SaveComponent
{ 
    public float px, py, pz;
    public float rx, ry, rz, rw;

    public SaveTransform(Transform transform)
	{
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        px = position.x;
        py = position.y;
        pz = position.z;

        rx = rotation.x;
        ry = rotation.y;
        rz = rotation.z;
        rw = rotation.w;
	}

    public Vector3 GetPosition()
	{
        return new Vector3(px, py, pz);
	}

    public Quaternion GetRotation()
	{
        return new Quaternion(rx, ry, rz, rw);
	}

    public override void AddComponent(GameObject gameObject)
    {
        gameObject.transform.position = GetPosition();
        gameObject.transform.rotation = GetRotation();
    }
}
