using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Construction : MonoBehaviour
{
	private SpriteRenderer sprite;
	private ConstructionCost cost;

	[SerializeField] int productionCost;
	[SerializeField] private int currentProduction;
	[SerializeField] Sprite[] construction1Sprites;
	[SerializeField] Sprite[] construction2Sprites;
	[SerializeField] Sprite[] construction3Sprites;
	[SerializeField] Sprite[] construction4Sprites;
	[SerializeField] Sprite[] finishedSprites;
	[SerializeField] bool isGhost;
	[SerializeField] bool canBuild;

	bool facingEast = true;

	Color yesGhostColor = new Color(0.8f,1f,1f,1f);

	bool inCollision;

	public bool CanBuild
	{
		get { return canBuild; }
	}

	public bool FacingEast
	{
		get { return facingEast; }
	}

	public bool IsGhost
	{
		get { return isGhost; }
		set
		{
			isGhost = value;
			if(!isGhost)
			{
				sprite.material.color = Color.white;
				GetComponent<NavMeshObstacle>().enabled = true;
				gameObject.layer = 11;
			}
		}
	}

	public void BeginConstruction(bool facingEast)
	{	
		cost.Spend();
		gameObject.layer = 11;
		IsGhost = false;
		this.facingEast = facingEast;

		sprite.sprite = Facing(ProductionStatus());
	}

	public void Construct(int productionPoints)
	{
		currentProduction += productionPoints;
		sprite.sprite = Facing(ProductionStatus());
		if(currentProduction >= productionCost)
		{
			if(GetComponent<Housing>() != null) GetComponent<Housing>().enabled = true;
		}
	}

	public void Flip()
	{
		facingEast = !facingEast;
		sprite.sprite = Facing(ProductionStatus());
	}

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		cost = GetComponent<ConstructionCost>();
	}

	private Sprite Facing(Sprite[] tierOfProduction)
	{
		if(facingEast || tierOfProduction.Length == 1)
			return tierOfProduction[0];
		else
			return tierOfProduction[1];
	}

	private void FixedUpdate()
	{
		if(isGhost)
		{
			Debug.Log(facingEast);
			if(!inCollision && cost.CanAfford())
			{
				canBuild = true;
			}
			else
				canBuild = false;
						
			sprite.material.color = canBuild ? yesGhostColor : Color.red;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if(isGhost && other.gameObject.layer != 14)
		{
			inCollision = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(isGhost && other.gameObject.layer != 14)
		{
			inCollision = false;
		}
	}

	private Sprite[] ProductionStatus() // janky
	{
		Debug.Log("current: "+currentProduction);
		Debug.Log("cost: "+productionCost);
		Debug.Log("%: "+(float)currentProduction/productionCost);
		if(isGhost || (float)currentProduction/productionCost >= 1)
			return finishedSprites;		
		else if(((float)currentProduction/productionCost) >= 0.75f &&
		(currentProduction/productionCost) < 1)
			return construction4Sprites;
		else if(((float)currentProduction/productionCost) >= 0.5f &&
		(currentProduction/productionCost) < 0.75f)
			return construction3Sprites;
		else if(((float)currentProduction/productionCost) >= 0.25f &&
		(currentProduction/productionCost) < 0.5f)
			return construction2Sprites;
		else
			return construction1Sprites;
		
		
	}
}