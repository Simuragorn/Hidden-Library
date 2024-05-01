using UnityEngine;
using UnityEngine.EventSystems;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float zoomSpeed = 1.0f;
    [SerializeField] private float minZoom = 1.0f;
    [SerializeField] private float maxZoom = 5.0f;

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(camera.orthographicSize - zoomDelta, minZoom, maxZoom);
        }
    }
}
