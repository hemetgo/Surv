using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTools
{
    public static bool OnView(Transform origin, Transform target, float range, float viewAngle)
    {
        Vector3 dir = target.position - origin.position;
        float distance = Vector3.Distance(origin.position, target.position);
        float angle = Vector3.SignedAngle(dir, origin.forward, Vector3.up);
        Physics.Raycast(origin.position, dir, out RaycastHit hit, distance);

        if (hit.collider)
        {
            if (hit.collider.CompareTag(target.gameObject.tag))
            {
                if (Mathf.Abs(angle) < viewAngle / 2)
                {
                    if (distance <= range)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
