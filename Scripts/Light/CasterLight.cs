using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterLight : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = true;
    }
}
