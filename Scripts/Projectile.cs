using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float velocity;

    private void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(DespawnTimer());
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * Time.deltaTime * velocity;
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
