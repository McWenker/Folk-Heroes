using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskUtil
{
	public static bool CheckLayerMask(LayerMask layersToHit, int layer)
    {
        return layersToHit == (layersToHit | (1 << layer));
    }

    public static int GetLayer(LayerMask layerMask)
    {
        return layerMask.value;
    }
}
