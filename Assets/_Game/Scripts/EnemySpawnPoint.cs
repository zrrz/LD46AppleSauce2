using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    void Start()
    {
        EnemyManager.Instance.AddSpawnPoint(this);
    }

#if UNITY_EDITOR

    static Mesh gizmoMesh;
    
    private void OnDrawGizmos()
    {
        if(gizmoMesh == null)
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            gizmoMesh = primitive.GetComponent<MeshFilter>().sharedMesh;
            DestroyImmediate(primitive);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireMesh(gizmoMesh, transform.position + Vector3.up/2f, transform.rotation, new Vector3(1f, 0.5f, 1f));

        //Draw arrow
        Vector3 lineStart = transform.position + Vector3.up * 0.75f;
        Vector3 lineEnd = lineStart + transform.forward;
        Vector3 leftPoint = lineStart + transform.forward * 0.8f + transform.right * 0.25f;
        Vector3 rightPoint = lineStart + transform.forward * 0.8f - transform.right * 0.25f;
        Gizmos.DrawLine(lineStart, lineEnd);
        Gizmos.DrawLine(lineEnd, leftPoint);
        Gizmos.DrawLine(lineEnd, rightPoint);
    }
#endif
}
