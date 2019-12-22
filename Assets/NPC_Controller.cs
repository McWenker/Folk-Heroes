using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    [SerializeField] Transform DEMO_goHere;

    void Awake()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = DEMO_goHere.position; 
    }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(Vector3.one);
    }
}
