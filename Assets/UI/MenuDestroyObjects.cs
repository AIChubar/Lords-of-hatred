using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script which is destroying objects with a special tag.
/// </summary>
public class MenuDestroyObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("DestroyInMenu");
        foreach(GameObject obj in objects)
            Destroy(obj);
    }
    
}
