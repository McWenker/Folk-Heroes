using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Construction : MonoBehaviour
{
	private SpriteRenderer sprite;
	private ConstructionCost cost;
	[SerializeField] int spriteIndex;
	[SerializeField] Sprite[] sprites;
	[SerializeField] bool isGhost;
	[SerializeField] bool canBuild;

	bool inCollision;

	public int SpriteIndex
	{
		get { return spriteIndex;}
		set
		{
			if(value < 0)
				value = sprites.Length - 1;
			if(value >= sprites.Length)
				value = 0;
			spriteIndex = value;
			sprite.sprite = sprites[spriteIndex];
		}
	}

	public bool CanBuild
	{
		get { return canBuild; }
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

	public void Construct(int spriteIndex)
	{	
		cost.Spend();
		GetComponent<Housing>().enabled = true;
		IsGhost = false;
		SpriteIndex = spriteIndex;
	}

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		cost = GetComponent<ConstructionCost>();
		SpriteIndex = spriteIndex;
	}

	private void FixedUpdate()
	{
		if(isGhost)
		{
			if(!inCollision && cost.CanAfford())
			{
				canBuild = true;
			}
			else
				canBuild = false;
						
			sprite.material.color = canBuild ? Color.white : Color.red;
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
}