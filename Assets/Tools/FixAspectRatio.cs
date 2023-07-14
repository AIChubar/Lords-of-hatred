using Cinemachine;
using UnityEngine;

/**
 * Force the Cinemachine camera to make viewport exactly as wide as the bounding object.
 * This allows us to always be 100% wide regardless of device. Handy for vertical scrollers.
 */
public class FixAspectRatio : MonoBehaviour {
    public int fullWidthUnits = 18;
    public int fullHeightUnits = 5;
    void Start () {
        // Force fixed width
        float ratio = (float)Screen.height / (float)Screen.width;
        float heightSize = fullHeightUnits / ratio / 4f;
        float widthSize = fullWidthUnits * ratio / 2f;
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = widthSize > heightSize  ? widthSize : heightSize;
    }
}