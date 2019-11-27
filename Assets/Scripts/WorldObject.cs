using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] protected WorldObjectScriptableObject data;
    private bool isPostDeath;
    protected WorldObject_Animator animator;

    public WorldObjectScriptableObject Data
    {
        get { return data; }
    }
    public bool IsPostDeath
    {
        get { return isPostDeath; }
    }
    public TileEffectTargetStruct[] Targetability
    {
        get { return data.targetability; }
    }

    // called when an worldobject is spawned in a tile, to give the worldobject information
    // like previous waterings, magic effects, etc
    public virtual void TileData(WorldTile thisTile)
    {
        data = thisTile.DefaultWorldObjectData;
        
		animator.GetData
			(data.damageFrameArray, data.deathFrameArray,
			data.shadowTrigger, data.postDeathSprite, data.postDeathFrameArray,
			data.frameRate, data.shadowParams, data.outlineColor);
        animator.SetSprite(data.sprite);
    }

    protected virtual void Awake()
    {
        AnimationEventManager.OnDeathAnimComplete += DeathAnimationComplete;
        animator = GetComponent<WorldObject_Animator>();
    }

    private void DeathAnimationComplete(Object sender)
    {
        if(this != null && sender == gameObject)
        {
            if(!data.hasPostDeath || isPostDeath)
            {
                Vector3 ensureZero = new Vector3(transform.position.x, 0, transform.position.z);// sigh, UnityEditor
                GridEventManager.WorldObjectRemove(this, Vector3Int.FloorToInt(ensureZero));
                Destroy(gameObject);
                return;
            }
            
            isPostDeath = true;
            GetComponent<Health>().Set(transform, 1);
        }
    }
}
