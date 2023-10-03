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
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.25F);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom < 8)
        {
            zoom += 1;
            Camera.main.orthographicSize = zoom;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            zoom = 5;
            Camera.main.orthographicSize = zoom;
            transform.position = new(0, 0, -10);
        }

    }
}
