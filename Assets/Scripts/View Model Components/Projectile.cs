using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamage
{
	[SerializeField] float despawnTime;
    [SerializeField] float velocity;
    int damage;

    Weapon creatorWeapon;
    Transform attackOrigin;
    Vector3 previousPos;
	int layerMask;
	LayerMask layersToHit;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public Weapon CreatorWeapon
    {
        get { return creatorWeapon; }
        set { creatorWeapon = value;}
    }

    public Transform AttackOrigin
    {
        get { return attackOrigin; }
        set { attackOrigin = value; }
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

            if (collisionHP != null && LayerMaskUtil.CheckLayerMask(layersToHit, collisionHP.gameObject.layer))
                collisionHP.ModifyHP(-damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
    public void SetLayerMask(LayerMask mask)
	{
		layersToHit = mask;
	}
}
