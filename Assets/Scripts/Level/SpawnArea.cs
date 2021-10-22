using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField]
    private bool showGizmo;

    [SerializeField]
    private Color gizmoColor = new Color(0.8f, 0, 0.5f, 0.5f);
    public Vector3 GetRandomPointWithin()
    {
        Vector3 size = transform.localScale / 2.0f;
        Vector3 randomPoint = new Vector3(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y),  Random.Range(-size.z, size.z));
        return transform.position + randomPoint;
    }

    void OnDrawGizmos()
    {
        if (!showGizmo)
        {
            return;
        }
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
