using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage;

    private void Awake()
    {
        Debug.DrawRay(transform.position, transform.right, Color.red, 2f);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        var ray = new Ray(transform.position, transform.right);
        if (Physics.Raycast(ray, out hit, 0.3f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            Debug.Log(hit.collider.gameObject);
            Health collisionHP = hit.collider.gameObject.GetComponent<Health>();
            if (collisionHP != null)
                collisionHP.ModifyHP(-damage);
            Destroy(gameObject);
        }
    }
}
