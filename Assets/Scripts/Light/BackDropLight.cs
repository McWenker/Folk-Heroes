using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDropLight : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Renderer>().receiveShadows = enabled;
    }
}
