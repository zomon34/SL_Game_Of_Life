using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float zoom = 5;
    Vector3 newPosition;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom > 1)
        {
            zoom -= 1;
            Camera.main.orthographicSize = zoom;
            newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.1F);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom < 101)
        {
            zoom += 1;
            Camera.main.orthographicSize = zoom;
        }

    }
}
