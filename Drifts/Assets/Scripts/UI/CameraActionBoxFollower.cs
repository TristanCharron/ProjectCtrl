using UnityEngine;
using System.Collections;

public class CameraActionBoxFollower : MonoBehaviour {

    [SerializeField]
    Transform[] targets;

    [SerializeField]
	float boundingBoxPadding = 2f, minimumOrthographicSize = 8f, zoomSpeed = 20f, speed = 2;

    public Vector3 CameraAjust;

    Camera currentCamera;

    void Awake()
    {
        currentCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Rect boundingBox = CalculateTargetsBoundingBox();
        //currentCamera.orthographicSize = CalculateOrthographicSize(boundingBox);
		transform.position = Vector3.Lerp(transform.position, CalculateCameraPosition(boundingBox), Time.deltaTime * speed);
    }

    /// <summary>
    /// Calculates a bounding box that contains all the targets.
    /// </summary>
    /// <returns>A Rect containing all the targets.</returns>
    Rect CalculateTargetsBoundingBox()
    {
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;

        foreach (Transform target in targets)
        {
            Vector3 position = target.position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.z);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.z);
        }

        return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
    }

    /// <summary>
    /// Calculates a camera position given the a bounding box containing all the targets.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A Vector3 in the center of the bounding box.</returns>
    Vector3 CalculateCameraPosition(Rect boundingBox)
    {
        Vector3 boundingBoxCenter = boundingBox.center;

        
		return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, boundingBoxCenter.z) + CameraAjust;
        
    }

    /// <summary>
    /// Calculates a new orthographic size for the camera based on the target bounding box.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A float for the orthographic size.</returns>
    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = currentCamera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, 0f , boundingBox.y);
        Vector3 topRightAsViewport = currentCamera.WorldToViewportPoint(topRight);

        if (topRightAsViewport.x >= topRightAsViewport.z)
            orthographicSize = Mathf.Abs(boundingBox.width) / currentCamera.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return Mathf.Clamp(Mathf.Lerp(currentCamera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, Mathf.Infinity);
    }
}

