using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housing : MonoBehaviour
{
	[SerializeField] int housingCount;
	[SerializeField] float spawnTimer;
	[SerializeField] float spawnCooldownTimer;
	[SerializeField] bool spawnInQueue;
	[SerializeField] bool spawnCooldown;
	[SerializeField] bool atCapacity;
	[SerializeField] GameObject unitToSpawn;
	[SerializeField] Transform spawnLoc;
	[SerializeField] ProgressBar_Radial progressBar;
	private float progress;
	private List<IUnit> spawnList;
	private void Awake()
	{
		spawnList = new List<IUnit>(housingCount);
		DeathEventManager.OnUnitDeath += CheckUnitDeath;
	}

	private void CheckUnitDeath(IUnit sender)
	{
		if(spawnList.Contains(sender))
		{
			spawnList.Remove(sender);
			
			if(spawnList.Count < housingCount)
				atCapacity = false;
		}
	}

	private void Spawn()
	{
		if(spawnList.Count < housingCount)
		{
			IUnit unit = GameObject.Instantiate(unitToSpawn, spawnLoc.position, Quaternion.identity).GetComponent<IUnit>();
			spawnList.Add(unit);
			unit.Idling();
			if(spawnList.Count >= housingCount)
				atCapacity = true;
		}
	}

	private void FixedUpdate()
	{
		if(!spawnCooldown && !spawnInQueue && !atCapacity)
		{
			progress = 0f;
			spawnCooldown = true;
			spawnInQueue = true;
			progressBar.gameObject.SetActive(true);
			progressBar.SetProgress(0, gameObject.transform);
			StartCoroutine(SpawnTimer());
		}
	}

	private IEnumerator SpawnCooldown()
	{
		yield return new WaitForSeconds(spawnCooldownTimer);
		spawnCooldown = false;
	}
	private IEnumerator SpawnTimer()
	{	
		float timer;
		for(timer = 0; timer < spawnTimer; timer += Time.deltaTime)
		{
			progressBar.SetProgress(100 * (timer / spawnTimer), gameObject.transform);
			yield return null;
		}
		progressBar.SetProgress(100 * (timer / spawnTimer), gameObject.transform);
		spawnInQueue = false;
		progressBar.gameObject.SetActive(false);
		Spawn();
		StartCoroutine(SpawnCooldown());
	}
}
