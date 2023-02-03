using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAgent : MonoBehaviour
{
    [SerializeField] private Waypoint waypoints;

    [SerializeField] private float moveSpeed = 5f;

    private Transform currentWaypoint;
    // Start is called before the first frame update
    void Start()
    {
        //Set initial pos to first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;
        
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
    }
}
