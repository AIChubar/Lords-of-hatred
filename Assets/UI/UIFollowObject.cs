using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowObject : MonoBehaviour
{
    public Transform objectToFollow;

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow != null)
        {
            Vector3 pivot = new Vector3(0, objectToFollow.gameObject.GetComponent<Renderer>().bounds.size.y/2f + 0.5f,1f);
            rectTransform.anchoredPosition = objectToFollow.localPosition + pivot;
        }
    }
}
