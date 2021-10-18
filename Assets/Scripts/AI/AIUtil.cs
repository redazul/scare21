using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public static class AIUtil
{
    /// <summary>
    /// Computes the distance between two 3D vectors, ignoring their y-Value
    /// </summary>
    public static float FlatDistance(Vector3 pos1, Vector3 pos2)
    {
        return Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));
    }

    public static Vector3 ApplyRandomXZDisplacement(Vector3 center, float radius)
    {
        if (radius == 0.0f)
        {
            return center;
        }

        float newX = Random.Range(center.x - radius, center.x + radius);
        float newZ = Random.Range(center.z - radius, center.z + radius);
        return new Vector3(newX, center.y, newZ);
    }
}