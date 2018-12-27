using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGravity : MonoBehaviour
{
    [SerializeField] float minY;

    private void FixedUpdate()
    {
        if (transform.position.y > minY)
            transform.position += Vector3.down * 0.01f;
    }
}
