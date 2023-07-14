using Cinemachine;
using UnityEngine;


/// <summary>
/// Force the Cinemachine camera to make viewport exactly as wide as the bounding object.
/// This allows us to always be 100% wide regardless of device. Handy for vertical scrollers.
/// </summary>
public class FixAspectRatio : MonoBehaviour {
    [SerializeField]
    private int fullWidthUnits = 18;
    
    [SerializeField]
    private int fullHeightUnits = 5;
    void Start () {
        // Force fixed width
        float ratio = Screen.height / (float)Screen.width;
        float heightSize = fullHeightUnits / ratio / 4f;
        float widthSize = fullWidthUnits * ratio / 2f;
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = widthSize > heightSize  ? widthSize : heightSize;
    }
}