using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
    public static bool GetInLineOfSight(Transform t1, Transform t2)
    {
        RaycastHit hit;
        if(Physics.Raycast(t1.position, t2.transform.position - t1.position, out hit))
        {
            if(hit.transform.root == t2.root)
            {
                return true;
            }
        }
        return false;//Comment
    }

    public static bool GetInLineOfSightAndRange(Transform t1, Transform t2, float maxRange)
    {
        RaycastHit hit;
        if (Physics.Raycast(t1.position, t2.transform.position - t1.position, out hit))
        {
            if (hit.transform.root == t2.root)
            {
                if(Vector3.Distance(t1.position, t2.position) <= maxRange)
                    return true;
            }
        }
        return false;
    }

    public static bool GetAbilityInRange(Transform t1, Transform t2, float maxRange)
    {
        if (Vector3.Distance(t1.position, t2.position) <= maxRange)
            return true;
        return false;
    }
}
