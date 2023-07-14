using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    private PlayerAnimation player;

    private float angleBetweenPlayerAndLaser;

    private LayerMask maskObstacle;

    private bool laserPreparing = true;
    
    public float AnglePerSecond;
    
    private PolygonCollider2D polygonCollider2D;

    private Vector2 distantLaserPoint; // used for ray always hitting the wall
    
    private float levelCoef = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        levelCoef += GameManager.gameManager.Character.levelsProgression[1] / 12f;
        if (levelCoef > 2.5f)
        {
            levelCoef *= 2.5f;
        }
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        distantLaserPoint = new Vector2(50f, 0f);
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        maskObstacle = LayerMask.GetMask("Obstacle");
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.enabled && !laserPreparing)
        {
            UpdateLaser();
        }
        else
        {
            polygonCollider2D.enabled = false;
        }
    }


    public void EnableLaser()
    {
        lineRenderer.enabled = true;
        StartCoroutine(LaserPreparation(2f));
    }

    private void UpdateLaser()
    {
        Vector2 laserDirection = distantLaserPoint - (Vector2)gameObject.transform.position;
        
        angleBetweenPlayerAndLaser = Vector3.Angle(laserDirection,
            player.transform.position - gameObject.transform.position);

        Vector3 cross = Vector3.Cross(laserDirection,
            player.transform.position - gameObject.transform.position);

        if (angleBetweenPlayerAndLaser > 1f)
        {
            if (cross.z > 0f)
            {
                laserDirection = Quaternion.AngleAxis(AnglePerSecond * Time.deltaTime, Vector3.forward) * laserDirection;
            }
            else
            {
                laserDirection = Quaternion.AngleAxis(-AnglePerSecond * Time.deltaTime, Vector3.forward) * laserDirection;
            }
        }
        

        distantLaserPoint = laserDirection + (Vector2)gameObject.transform.position;
        lineRenderer.SetPosition(1, distantLaserPoint);
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, laserDirection.normalized, laserDirection.magnitude, maskObstacle);
        if (hit)
        {
             lineRenderer.SetPosition(1, hit.point);
        }
    }

    public void DisableLaser()
    {
        lineRenderer.enabled = false;
    }
    public Vector3[] GetPositions() {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        return positions;
    }

    public float GetWidth() {
        return lineRenderer.startWidth;
    }

    private IEnumerator LaserPreparation(float duration)
    {
        laserPreparing = true;
        polygonCollider2D.enabled = false;
        lineRenderer.startColor = new Color(0f,0f,0f,0.4f);
        lineRenderer.endColor = new Color(0f,0f,0f,0.4f);
        
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            Vector3 gameObjectPos = gameObject.transform.position;
            distantLaserPoint = (player.transform.position - gameObjectPos).normalized * 50f;
            lineRenderer.SetPosition(1, (Vector3)distantLaserPoint + gameObjectPos);
            RaycastHit2D hit = Physics2D.Raycast(gameObjectPos, distantLaserPoint.normalized, distantLaserPoint.magnitude, maskObstacle);
            if (hit)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            yield return null;
        }
        
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            lineRenderer.startColor = new Color(Mathf.SmoothStep(0, 1, t),Mathf.SmoothStep(0, 1, t),Mathf.SmoothStep(0, 1, t),Mathf.SmoothStep(0.4f, 1, t));
            lineRenderer.endColor = new Color(Mathf.SmoothStep(0, 1, t),Mathf.SmoothStep(0, 1, t),Mathf.SmoothStep(0, 1, t),Mathf.SmoothStep(0.4f, 1, t));
            yield return null;
        }
        
        polygonCollider2D.enabled = true;
        laserPreparing = false;
    
    }
}
