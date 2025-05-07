using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyNavMesh : MonoBehaviour
{
    public Transform[] Waypoints;
    private NavMeshAgent agente;
    
    private int i = 0;
    
    private Boolean isOnDirection = false;
    private Boolean isRotated = false;
    
    private bool isWaiting = false;
    private float waitTimer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
            }
            return; // No hacer nada mientras esperamos
        }
        
        if (isOnDirection == false)
        {
            RotationToTheWaypoint(i);
            return; 
        }

        agente.SetDestination(Waypoints[i].position);

        // Verificar si llegó al waypoint
        if (HasReachedDestination() == true)
        {
            if (isRotated == false)
            {
                RotationOnTheWaypoint(i);
                return; 
            }
            
            // Preparar para el siguiente waypoint
            StartWaiting();
        }
    }

    bool HasReachedDestination()
    {
        return agente.remainingDistance <= agente.stoppingDistance && 
               (!agente.hasPath || agente.velocity.sqrMagnitude == 0f);
    }
    
    void StartWaiting()
    {
        // Obtener el tiempo de espera del waypoint actual
        Waypoint waypointData = Waypoints[i].GetComponent<Waypoint>();
        
        if (waypointData != null)
        {
            waitTimer = waypointData.TimeOnThisWaypoint;
            isWaiting = true;
        }
    
        // Preparar el siguiente waypoint (pero no moverse hasta que termine la espera)
        PrepareNextWaypoint();
    }

    void PrepareNextWaypoint()
    {
        isOnDirection = false;
        isRotated = false;
        
        i++;
        if (i >= Waypoints.Length) {
            i = 0;
        }
        
    }

   
    void RotationToTheWaypoint(int waypointIndex)
    {
        Vector3 direction = Waypoints[waypointIndex].position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
    
        if (angleDifference < 1f)
        {
            isOnDirection = true;
        }
    }

    void RotationOnTheWaypoint(int waypointIndex)
    {
        Quaternion targetRotation = Waypoints[waypointIndex].rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
        
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
    
        if (angleDifference < 1f)
        {
            isRotated = true;
        }
    }
    
}
