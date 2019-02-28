using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionHandler : MonoBehaviour
{
	[SerializeField] ControlStateHandler controlStateHandler;
	[SerializeField] GameObject toConstructPrefab;
	Construction toConstruct;

    private bool constructionCooldown;
    private bool constructionFlipCooldown;

	private void Awake()
	{		
		InputEventManager.OnFire += Build;
		InputEventManager.OnAbilityUse += FlipSprite;
	}
	private void Build(Object sender, int buttonFired)
	{
		if(buttonFired == 0)
		{
			if(!constructionCooldown)
			{
				if(toConstruct != null && toConstruct.CanBuild)
				{
					Construction construct = GameObject.Instantiate(toConstruct, toConstruct.transform.position, Quaternion.identity).GetComponent<Construction>();
					construct.Construct(toConstruct.SpriteIndex);
					constructionCooldown = true;
					StartCoroutine(ConstructionCooldown());
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if(toConstruct != null)
		{
			toConstruct.transform.position = RayToGroundUtil.FetchMousePointOnGround(0f);
		}
	}

	private void FlipSprite(Object sender, string keyPress)
	{
		if((keyPress != "Q" && keyPress != "E") || constructionFlipCooldown)
			return;
		else if(toConstruct.IsGhost)
		{
			toConstruct.SpriteIndex++;
			constructionFlipCooldown = true;
			StartCoroutine(ConstructionFlipCooldown());
		}
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
		toConstructPrefab = toConstruct;
		SpawnGhost();
	}
	
    private IEnumerator ConstructionCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        constructionCooldown = false;
    }

    private IEnumerator ConstructionFlipCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        constructionFlipCooldown = false;
    }
}
