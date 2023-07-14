using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=BfP0KyOxVWs

/// <summary>
/// Script responsible for generating collision shape for a thick line. 
/// </summary>
[RequireComponent(typeof(Laser),typeof(PolygonCollider2D))]
public class LineCollision : MonoBehaviour
{
    private Laser laser;

    private PolygonCollider2D polygonCollider2D;

    //The points to draw a collision shape between
    private List<Vector2> colliderPoints = new List<Vector2>(); 

    void Awake()
    {
        laser = GetComponent<Laser>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (laser && polygonCollider2D.enabled)
        {
            colliderPoints = CalculateColliderPoints();
            polygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        }
    }
    
    private List<Vector2> CalculateColliderPoints() {
        //Get All positions on the line renderer
        Vector3[] positions = laser.GetPositions();

        //Get the Width of the Line
        float width = laser.GetWidth();

        //m = (y2 - y1) / (x2 - x1)
        float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        //Calculate the Offset from each point to the collision vertex
        Vector3[] offsets = new Vector3[2];
        offsets[0] = new Vector3(-deltaX, deltaY);
        offsets[1] = new Vector3(deltaX, -deltaY);

        //Generate the Colliders Vertices
        List<Vector2> colliderPositions = new List<Vector2> {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return colliderPositions;
    }
}
