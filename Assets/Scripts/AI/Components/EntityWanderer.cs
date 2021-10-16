using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EntityWanderer : MonoBehaviour
{
    private const float WAYPOINT_REACHED_RANGE = 0.25f;

    [Tooltip("Use a random displacement for the waypoint")]
    [SerializeField]
    private float displacementRadius = 2.0f;

    [SerializeField]
    private bool useRandomWaypointOrder = true;

    [SerializeField]
    private List<Vector3> waypoints = new List<Vector3>();

    [SerializeField]
    private int nextWaypointIndex = 0;

    [SerializeField]
    private float minWaitBetweenMovement = 1.0f;

    [SerializeField]
    private float maxWaitBetweenMovement = 3.0f;

    [SerializeField]
    private bool displayGizmo = true;

    private Vector3? nextWaypoint;

    private NavMeshPath currentPath;
    private bool pathDirty = false;

    private NavMeshAgent navMeshAgent;

    private Timer timer = null;

    void Awake()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if(navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        timer = gameObject.AddComponent<Timer>();
    }

    void Update()
    {
        CheckAndUpdateWaypoints();
        UpdateNavMeshPath();

    }

    private void PauseAfterTargetReached()
    {
        StopDefaultMovement();
    }

    private void StartDefaultMovement()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetPath(currentPath);
    }
    private void StopDefaultMovement()
    {
        navMeshAgent.isStopped = true;
    }

    private void CheckAndUpdateWaypoints()
    {
        if (!nextWaypoint.HasValue)
        {
            //make sure theres no invalid waypoint position
            UpdateNextWaypoint();
        }
        else if (FlatDistance(transform.position, nextWaypoint.Value) < WAYPOINT_REACHED_RANGE && !timer.IsRunning())
        {
            //wait for some time before going to the next waypoint
            timer.Init(Random.Range(minWaitBetweenMovement, maxWaitBetweenMovement), UpdateNextWaypoint);
        }
    }

    private void UpdateNavMeshPath()
    {
        if (!pathDirty)
        {
            return;
        }
        if (!nextWaypoint.HasValue)
        {
            Debug.LogWarning("No nextWaypoint was assigned. Will not update the current NavMeshPath");
            return;
        }
        currentPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, nextWaypoint.Value, NavMesh.AllAreas, currentPath);
        pathDirty = false;

        if (currentPath.status != NavMeshPathStatus.PathInvalid)
        {
            StartDefaultMovement();
        }
    }

    /// <summary>
    /// Returns the next waypoint, either by list order or random, depending on useRandomWaypointOrder.
    /// </summary>
    /// 
    private void UpdateNextWaypoint()
    {
        //check if there are waypoints
        if (waypoints.Count == 0)
        {
            Debug.LogError("Can't get the next waypoint because no waypoints were defined.");
        }

        //save the current waypoint index and calculate the waypoint index for the next call
        int currentWaypointIndex = nextWaypointIndex;
        if (useRandomWaypointOrder)
        {
            nextWaypointIndex = GetNextRandomWaypointIndex(waypoints.Count, currentWaypointIndex);
        }
        else
        {
            //cycle through waypoints
            nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Count - 1;
        }

        Vector3 targetWaypoint = ApplyRandomXZDisplacement(waypoints[currentWaypointIndex], displacementRadius);


        float navMeshSearchRadius = 1.0f;
        NavMeshHit closestNavMeshPosition;
        bool foundPos = NavMesh.SamplePosition(targetWaypoint, out closestNavMeshPosition, navMeshSearchRadius, NavMesh.AllAreas);
        if (!foundPos)
        {
            Debug.LogWarning("Could not find a nav mesh position within " + navMeshSearchRadius + "f for given point " + targetWaypoint);
            nextWaypointIndex = currentWaypointIndex;
            nextWaypoint = null;
            return;
        }

        pathDirty = true;
        nextWaypoint = closestNavMeshPosition.position;
    }

    private static int GetNextRandomWaypointIndex(int numWaypoints, int currentWaypointIndex)
    {
        //singular entry => return index 0
        if(numWaypoints == 1)
        {
            return 0;
        }

        //get some random waypoint except for the current index
        List<int> possibleIndices = Enumerable.Range(0, numWaypoints - 1).ToList()
            .Except(new List<int>(1) { currentWaypointIndex })
            .ToList();
        return possibleIndices[Random.Range(0, possibleIndices.Count)];
    }


    /// <summary>
    /// Computes the distance between two 3D vectors, ignoring their y-Value
    /// </summary>
    private static float FlatDistance(Vector3 pos1, Vector3 pos2)
    {
        return Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));
    }


    private static Vector3 ApplyRandomXZDisplacement(Vector3 center, float radius)
    {
        if(radius == 0.0f)
        {
            return center;
        }

        float newX = Random.Range(center.x - radius, center.x + radius);
        float newZ = Random.Range(center.z - radius, center.z + radius);
        return new Vector3(newX, center.y, newZ);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!displayGizmo)
        {
            return;
        }
        DrawTargetWaypointGizmo();
        DrawNextTargetWaypointGizmo();
        DrawPathGizmo();
    }

    private void DrawTargetWaypointGizmo()
    {

        if (nextWaypoint.HasValue)
        {
            Gizmos.color = Gizmos.color = new Color(0.4f, 0.2f, 0.9f, 0.6f);
            Gizmos.DrawSphere(nextWaypoint.Value, 0.8f);
        }
    }

    private void DrawNextTargetWaypointGizmo()
    {
        if (waypoints.Count > 0)
        {
            Gizmos.color = new Color(0.2f, 0.2f, 0.9f, 0.3f);
            Gizmos.DrawSphere(waypoints[nextWaypointIndex], 0.8f+ displacementRadius);
        }
    }

    private void DrawPathGizmo()
    {
        if (currentPath != null && currentPath.status != NavMeshPathStatus.PathInvalid)
        {
            Gizmos.color = Gizmos.color = new Color(0.4f, 0.3f, 0.7f, 0.5f);
            for (int i = 0; i < currentPath.corners.Length-1; i++)
            {
                Gizmos.DrawLine(currentPath.corners[i], currentPath.corners[i+1]);
            }
        }
    }
#endif

}
