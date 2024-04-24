using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    private NavMeshAgent agent; // the spider's NavMeshAgent component
    private Transform playerTransform; // transform component of the player
    private Transform nearestBuilding; // transform component of the nearest building
    public bool isActive = true; // boolean flag to control if the spider should move

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // find the player object by name
        GameObject player = GameObject.Find("player"); // ensure the player's GameObject name is "player"
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("player object not found in the scene!");
        }

        // initialize the nearest building transform
        FindNearestBuilding();
    }

    void Update()
    {
        if (!isActive)
        {
            return; // if the spider should not be moving, exit the update method
        }

        if (playerTransform == null)
        {
            Debug.LogError("player transform is not set!");
            return;
        }

        FindNearestBuilding();
        // decide the destination based on the presence of a building and distances
        if (nearestBuilding != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            float distanceToBuilding = Vector3.Distance(transform.position, nearestBuilding.position);

            if (distanceToBuilding < distanceToPlayer)
            {
                agent.SetDestination(nearestBuilding.position);
            }
            else
            {
                agent.SetDestination(playerTransform.position);
            }
        }
        else
        {
            // if no buildings are found, default to moving towards the player
            agent.SetDestination(playerTransform.position);
        }
    }

    void FindNearestBuilding()
    {
        List<GameObject> buildings = allBuildings.Instance.buildings;
        if (buildings.Count == 0)
        {
            Debug.LogWarning("no buildings found in the allBuildings list!");
            nearestBuilding = null; // explicitly set to null if no buildings are present
            return;
        }

        float minDistance = Mathf.Infinity;
        Transform closestBuilding = null;

        foreach (GameObject building in buildings)
        {
            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = building.transform;
            }
        }

        nearestBuilding = closestBuilding;
    }
}
