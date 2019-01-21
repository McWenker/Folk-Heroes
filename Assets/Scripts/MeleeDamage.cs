using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour, IDamage
{
	[SerializeField] float despawnTime;
	int damage;

	private List<Health> hitList = new List<Health>();
	public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
	private void Awake()
    {
        StartCoroutine(DespawnTimer());
    }

	private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
		hitList.Clear();
        Destroy(gameObject);
    }

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.layer == 13) // enemy layer, TODO fix later
		{
			Health targetHP = other.GetComponent<Health>();
			if(targetHP != null && !hitList.Contains(targetHP))
			{
                targetHP.ModifyHP(-damage);
				hitList.Add(targetHP);
			}
		}
	}
}
