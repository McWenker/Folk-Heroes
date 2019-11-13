using UnityEngine;

public static class RayToGroundUtil
{
	public static Vector3 FetchMousePointOnGround(float planeHeight)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane hPlane = new Plane(Vector3.up, new Vector3(0, 0, planeHeight));
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

    public static Ray FetchRayToGround()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    /*public static Selectable FetchFirstSelectableHit()
    {
        RaycastHit hitInfo;
        //Shoots a ray into the 3D world starting at our mouseposition
        if (Physics.Raycast(FetchRayToGround(), out hitInfo))
        {
            //We check if we clicked on an object with a Selectable component
            return hitInfo.collider.GetComponentInParent<Selectable>();
        }
        else return null;
    }*/
}
