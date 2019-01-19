using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionHandler : MonoBehaviour
{
	[SerializeField] GameObject toConstructPrefab;
	Construction toConstruct;

	private void Awake()
	{		
		ConstructionEventManager.OnBuild += Build;
		ConstructionEventManager.OnSpriteFlip += FlipSprite;
	}
	private void Build(GameObject sender)
	{
		if(toConstruct != null && toConstruct.CanBuild)
		{
			Construction construct = GameObject.Instantiate(toConstruct, toConstruct.transform.position, Quaternion.identity).GetComponent<Construction>();
			construct.Construct(toConstruct.SpriteIndex);
		}
	}

	private void FixedUpdate()
	{
		if(toConstruct != null)
		{
			toConstruct.transform.position = RayToGroundUtil.FetchMousePointOnGround(0f);
		}
	}

	private void FlipSprite(GameObject sender)
	{
		if(toConstruct.IsGhost)
			toConstruct.SpriteIndex++;
	}

	private void OnDisable()
	{
		if(toConstruct != null)
		{
			GameObject.Destroy(toConstruct.gameObject);
			toConstruct = null;
		}
		toConstructPrefab = null;
	}
	private void SpawnGhost()
	{
		if(toConstructPrefab != null)
		{
			toConstruct = GameObject.Instantiate(toConstructPrefab, RayToGroundUtil.FetchMousePointOnGround(0f), Quaternion.identity).GetComponent<Construction>();
		}
	}
	public void SetConstruction(GameObject toConstruct)
	{
		if(this.toConstruct != null)
		{
			GameObject.Destroy(this.toConstruct.gameObject);
			this.toConstruct = null;
		}	
		ControlEventManager.ControlStateSet(ControlState.Construction);
		toConstructPrefab = toConstruct;
		SpawnGhost();
	}
}
