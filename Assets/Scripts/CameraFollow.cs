using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 1f, -10f);
    // Zoom at the reference aspect ratio 
    public float baseSize = 4f; 
    // Aspect the levels were designed for 
    public float referenceAspect = 16f / 9f; 

    void LateUpdate()
    {
        // Keep at least 16:9 on narrow screens 
        float aspect = (float)Screen.width / Screen.height;
        float size = aspect >= referenceAspect ? baseSize : baseSize * (referenceAspect / aspect);
        Camera.main.orthographicSize = size;

        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
