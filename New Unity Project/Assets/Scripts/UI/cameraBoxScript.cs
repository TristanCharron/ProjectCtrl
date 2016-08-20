using UnityEngine;
using System.Collections;

public class cameraBoxScript : MonoBehaviour {

    public GameObject player;
    [SerializeField]
    float zoomSpeed = 20f, boundingBoxPadding;

    float minOrthoSize, maxOrthoSize;

    float minX, maxX, minY, maxY;

    Rect boundingBox;


    void Awake()
    {

        minOrthoSize = Camera.main.orthographicSize;
        maxOrthoSize = minOrthoSize * 2;
        
    }

    void Update()
    {
        
        
            /*
            onCalculateTargetsBoundingBox();

            Camera.main.transform.position = onCalculateCameraPosition();
            Camera.main.orthographicSize = onCalculateOrthographicSize();


            Camera.main.transform.position = onCalculateCameraPosition();
            */
        
        onCalculateTargetsBoundingBox();

        Camera.main.transform.position = onCalculateCameraPosition();
        Camera.main.orthographicSize = onCalculateOrthographicSize();


          Camera.main.transform.position = onCalculateCameraPosition();
    }

    /// <summary>
    /// Calculates a bounding box that contains all the targets.
    /// </summary>
    /// <returns>A Rect containing all the targets.</returns>
    void onCalculateTargetsBoundingBox()
    {
        minX = Mathf.Infinity;
        maxX = Mathf.NegativeInfinity;
        minY = Mathf.Infinity;
        maxY = Mathf.NegativeInfinity;

        /*    for (int i = 0; i < combatManager.PlayerList.Length; i++)
            {
                if (combatManager.PlayerList[i] != null)
                {
                    if (combatManager.PlayerList[i].isAlive)
                    {

            */
        // Vector3 position = combatManager.PlayerList[i].gameObject.transform.position;
        Vector3 position = player.gameObject.transform.position;

        minX = Mathf.Min(minX, position.x);
        minY = Mathf.Min(minY, position.y);
        maxX = Mathf.Max(maxX, position.x);
        maxY = Mathf.Max(maxY, position.y);

        /*
                }

            }
        }

        */




        boundingBox = Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);

    }

    /// <summary>
    /// Calculates a camera position given the a bounding box containing all the targets.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A Vector3 in the center of the bounding box.</returns>
    Vector3 onCalculateCameraPosition()
    {
        Vector2 boundingBoxCenter = boundingBox.center;

        return new Vector3(boundingBoxCenter.x, Camera.main.transform.position.y, boundingBoxCenter.y);
    }

    /// <summary>
    /// Calculates a new orthographic size for the camera based on the target bounding box.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A float for the orthographic size.</returns>
    float onCalculateOrthographicSize()
    {

        float orthographicSize = Camera.main.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, Camera.main.transform.position.y, boundingBox.y);
        Vector3 topRightAsViewport = Camera.main.WorldToViewportPoint(topRight);

        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / Camera.main.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        float orthoPosition = Mathf.Lerp(Camera.main.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed);

        return Mathf.Clamp(orthoPosition, minOrthoSize, maxOrthoSize);
    }

}

