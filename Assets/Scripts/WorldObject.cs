using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] Item[] itemProducts;
    [SerializeField] private bool hasPostDeath;
    private bool isPostDeath;
    [SerializeField] private TileEffectTargetStruct[] targetability;

    public bool IsPostDeath
    {
        get { return isPostDeath; }
    }

    public TileEffectTargetStruct[] Targetability
    {
        get { return targetability; }
    }

    // called when an worldobject is spawned in a tile, to give the worldobject information
    // like previous waterings, magic effects, etc
    public virtual void TileData(WorldTile thisTile)
    {

    }

    private void Awake()
    {
        AnimationEventManager.OnDeathAnimComplete += DeathAnimationComplete;
    }

    private void DeathAnimationComplete(Object sender)
    {
        if(this != null && sender == gameObject)
        {
            if(!hasPostDeath || isPostDeath)
            {
                GridEventManager.WorldObjectRemove(this, Vector3Int.FloorToInt(transform.position));
                Destroy(gameObject);
                return;
            }
            
            isPostDeath = true;
            GetComponent<Health>().Set(transform, 1);
        }
    }
}
