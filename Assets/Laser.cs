using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer LineRenderer;
    
    private PlayerAnimation player;
    
    public float AngularSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimation>();
        LineRenderer.SetPosition(0, gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (LineRenderer.enabled)
        {
            UpdateLaser();
            
        }
    }


    public void EnableLaser()
    {
        LineRenderer.enabled = true;
    }

    private void UpdateLaser()
    {
        LineRenderer.SetPosition(1, player.transform.position);
    }

    public void DisableLaser()
    {
        LineRenderer.enabled = false;
    }


}
