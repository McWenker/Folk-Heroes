using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskUtil
{
	public static int GetLayers(LayerMask[] layersToHit)
    {
        int retVal = 0;
        for(int i = 0; i < layersToHit.Length; ++i)
        {
            retVal |= layersToHit[i].value;
        }
        return retVal;
    }
}
