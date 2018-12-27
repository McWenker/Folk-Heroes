using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage;
    Vector3 previousPos;

    private void Start()
    {
        previousPos = transform.position;
    }

    private void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(previousPos, (transform.position - previousPos).normalized), (transform.position - previousPos).magnitude);
        previousPos = transform.position;

        for (int i = 0; i < hits.Length; i++)
        {
            Health collisionHP = hits[i].collider.gameObject.GetComponent<Health>();

            if (collisionHP != null)
                collisionHP.ModifyHP(-damage);
            Destroy(gameObject);
        }

        /*
        RaycastHit hit;
        var ray = new Ray(transform.position, transform.right);
        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            Health collisionHP = hit.collider.gameObject.GetComponent<Health>();
            if (collisionHP != null)
                collisionHP.ModifyHP(-damage);
            Destroy(gameObject);
        }
        */
    }
}
