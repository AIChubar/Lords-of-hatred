using UnityEngine;

/// <summary>
/// Script that should be attached to UI element which should follow some GameObject.
/// </summary>
public class UIFollowObject : MonoBehaviour
{
    [SerializeField]
    private Transform objectToFollow;

    private RectTransform rectTransform;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (objectToFollow != null)
        {
            Vector3 pivot = new Vector3(0, objectToFollow.gameObject.GetComponent<Renderer>().bounds.size.y/2f + 0.5f,1f);
            rectTransform.anchoredPosition = objectToFollow.localPosition + pivot;
        }
    }
}
