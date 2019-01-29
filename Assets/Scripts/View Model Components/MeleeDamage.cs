using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour, IDamage
{
	[SerializeField] float despawnTime;
	int damage;
	Weapon creatorWeapon;
	Transform attackOrigin;
	LayerMask layersToHit;
	private List<Health> hitList = new List<Health>();
	public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

	public Weapon CreatorWeapon
	{
        get { return creatorWeapon; }
        set { creatorWeapon = value; }
	}

	public Transform AttackOrigin
	{
		get { return attackOrigin; }
		set { attackOrigin = value; }
	}
	private void Awake()
    {
        StartCoroutine(DespawnTimer());
    }

	private void FixedUpdate()
	{
		transform.position = attackOrigin.position;
	}

	private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
		hitList.Clear();
        Destroy(gameObject);
    }

	private void OnTriggerStay(Collider other)
	{
		if(LayerMaskUtil.CheckLayerMask(layersToHit, other.gameObject.layer)) // enemy layer, TODO fix later
		{
			Health targetHP = other.GetComponent<Health>();
			if(targetHP != null && !hitList.Contains(targetHP))
			{
                targetHP.ModifyHP(-damage);
				hitList.Add(targetHP);
			}
		}
	}

	public void SetLayerMask(LayerMask mask)
	{
		layersToHit = mask;
	}
}
