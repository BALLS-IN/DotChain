using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityScript {

	public static Vector3 GenerateRotatedPoint (Vector3 from, float angle)
	{
		Vector3 p = Quaternion.Euler(0, angle, 0) * from;
		return p;
	}

	public static int RandomSign()
	{
     	if (Random.Range(0, 2) == 0) return -1;
        return 1;
    }

	public static void SetColliderActive (Collider[] c, bool active)
	{
		for (int i=0; i<c.Length; i++)
		{
			c[i].enabled = active;
		}
	}
}
