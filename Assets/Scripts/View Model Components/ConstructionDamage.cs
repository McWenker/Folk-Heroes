using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionDamage : MonoBehaviour, IDamage
{
	
	[SerializeField] float despawnTime;
	int damage;
	Weapon creatorWeapon;
	Transform attackOrigin;
	LayerMask layersToHit;
	private List<Construction> hitList = new List<Construction>();
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
		if(gameObject != null)
        	Destroy(gameObject);
    }

	private void OnTriggerStay(Collider other)
	{
		if(LayerMaskUtil.CheckLayerMask(layersToHit, other.gameObject.layer)) // enemy layer, TODO fix later
		{
			Construction targetConstruct = other.GetComponent<Construction>();
			if(targetConstruct != null && !hitList.Contains(targetConstruct))
			{
                targetConstruct.Construct(damage);
				hitList.Add(targetConstruct);
			}
		}
	}

	public void SetLayerMask(LayerMask mask)
	{
		layersToHit = mask;
	}
}
