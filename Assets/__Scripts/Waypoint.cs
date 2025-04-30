using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoint : MonoBehaviour
{
    public Transform[] Waypoints;
    public int TimeForMovement;
    private NavMeshAgent agente;

    
    
    // Start is called before the first frame update
    void Start()
    {
        agente = gameObject.GetComponent<NavMeshAgent>();

        Vector3[] posiciones = new Vector3[Waypoints.Length];
        Quaternion[] rotaciones = new Quaternion[Waypoints.Length];
        
        for (int i = 0; i < Waypoints.Length; i++)
        {
            posiciones[i] = Waypoints[i].position;
            rotaciones[i] = Waypoints[i].rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
