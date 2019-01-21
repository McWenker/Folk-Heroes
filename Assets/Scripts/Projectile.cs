using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamage
{
	[SerializeField] float despawnTime;
    [SerializeField] float velocity;
    int damage;
    Vector3 previousPos;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Awake()
    {
        previousPos = transform.position;
        GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(DespawnTimer());
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * Time.deltaTime * velocity;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(previousPos, (transform.position - previousPos).normalized), (transform.position - previousPos).magnitude);
        previousPos = transform.position;

        for (int i = 0; i < hits.Length; i++)
        {
            Health collisionHP = hits[i].collider.gameObject.GetComponent<Health>();

            if (collisionHP != null)
                collisionHP.ModifyHP(-damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
