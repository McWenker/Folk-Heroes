using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] protected WorldObjectScriptableObject data;
    private bool isPostDeath;

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

    }

    protected virtual void Awake()
    {
        AnimationEventManager.OnDeathAnimComplete += DeathAnimationComplete;
        
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
