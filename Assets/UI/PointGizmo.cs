using UnityEngine;

public class PointGizmo : MonoBehaviour
{
    public float radius;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
