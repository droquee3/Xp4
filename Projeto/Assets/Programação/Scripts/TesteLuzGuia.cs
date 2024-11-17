using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteLuzGuia : MonoBehaviour
{
    public Transform player;
    public float distanceAhead = 5f;
    public float heightOffset = 2f;
    public float smoothTime = 0.3f;
    public bool followPlayer = true;

    public Transform[] waypoints;
    public float waypointSpeed = 3f;
    public float waypointThreshold = 0.1f;
    public int maxWaypoints = 5;
    public float playerProximityThreshold = 2f;
    public float rotationSpeed = 2f;
    private Vector3 currentVelocity;
    private int currentWaypointIndex = 0;
    private int waypointsTraversed = 0;
    private bool isDeactivating = false;
    private float deactivateTimer = 0f;
    private bool waitingForPlayer = false;

    void Update()
    {
        if (followPlayer)
        {
            Vector3 targetPosition = player.position + player.forward * distanceAhead + Vector3.up * heightOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
        else
        {
            if (waypointsTraversed < maxWaypoints)
            {
                MoveAlongWaypoints();
            }
            else if (!isDeactivating)
            {
                isDeactivating = true;
            }
        }

        if (isDeactivating)
        {
            deactivateTimer -= Time.deltaTime;
            if (deactivateTimer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void MoveAlongWaypoints()
    {
        if (waypoints.Length == 0) return;

        if (waitingForPlayer)
        {
            if (Mathf.Abs(player.position.x - transform.position.x) <= 5f) 
            {
                waitingForPlayer = false; 
            }
            else
            {
                return; 
            }
        }

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, waypointSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= waypointThreshold)
        {

            if (currentWaypointIndex == 4 || currentWaypointIndex == 6)
            {
                waitingForPlayer = true;
            }
            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }
}