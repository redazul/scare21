using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityWanderer : MonoBehaviour
{
    [SerializeField]
    private float displacementRadius = 2.0f;

    [SerializeField]
    private bool useRandomWaypointOrder = true;

    [SerializeField]
    private List<Vector3> waypoints = new List<Vector3>();

    [SerializeField]
    private int nextWaypointIndex = 0;

    private Vector3? targetWaypoint;


    /// <summary>
    /// Returns the next waypoint, either by list order or random, depending on useRandomWaypointOrder.
    /// </summary>
    /// 
    private Vector3 GetNextWaypoint()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogError("Can't get the next waypoint because no waypoints were defined.");
        }

        int currentWayPointIndex = nextWaypointIndex;

        if (useRandomWaypointOrder)
        {
            //get some random waypoint except for the current index
            List<int> possibleIndices = Enumerable.Range(0, waypoints.Count - 1).ToList()
                .Except(new List<int>(1) { nextWaypointIndex })
                .ToList();
            nextWaypointIndex = possibleIndices[Random.Range(0, possibleIndices.Count)];
        }
        else
        {
            //cycle through waypoints
            nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Count - 1;
        }

        return waypoints[currentWayPointIndex];
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawTargetWaypointGizmo();
        DrawNextTargetWaypointGizmo();
    }

    private void DrawTargetWaypointGizmo()
    {
        if (targetWaypoint.HasValue)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(targetWaypoint.Value, 0.8f);
        }
    }

    private void DrawNextTargetWaypointGizmo()
    {
        if (waypoints.Count > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(waypoints[nextWaypointIndex], 0.8f);
        }
    }

#endif

}
