using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayToGroundUtil
{
	public static Vector3 FetchMousePointOnGround(float planeHeight)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane hPlane = new Plane(Vector3.up, new Vector3(0, planeHeight, 0));
        float distance = 0;

        // if the ray hits the plane...
        if (hPlane.Raycast(ray, out distance))
        {
            // get the hit point:
            Debug.DrawRay(ray.GetPoint(distance), Vector3.up, Color.red);
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
