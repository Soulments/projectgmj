using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDebugLineObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
