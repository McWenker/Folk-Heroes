using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDestination : MonoBehaviour
{
    [SerializeField] string destination;

    public string Destination
    {
        get { return destination; }
    }
}
