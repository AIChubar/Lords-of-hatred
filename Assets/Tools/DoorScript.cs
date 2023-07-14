using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public SpriteMask mask;
    public Collider2D col;
    
    // Start is called before the first frame update
    void Start()
    {
        col.enabled = false;
    }

    public void OpenDoor()
    {
        StartCoroutine(DoorOpening());
    }
    
    private IEnumerator DoorOpening()
    {
        col.enabled = false;
        float initX = mask.transform.position.x;
        for (float t = 0; t < 1; t += Time.deltaTime / 2)
        {
            mask.transform.Translate(Time.deltaTime / 2, 0, 0);
            yield return null;
        }

        col.enabled = true;
    }
}
