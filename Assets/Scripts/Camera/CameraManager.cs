using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float movementSpeed = 20f;
    [SerializeField] private Transform topLeftCorner;
    [SerializeField] private Transform topRightCorner;
    [SerializeField] private Transform bottomLeftCorner;
    [SerializeField] private Transform bottomRightCorner;
    [SerializeField] private float verticalOffset = 2f;
    [SerializeField] private float cameraSizeForOffset = 15f;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        Vector2 direction = target.position - transform.position;
        direction += Vector2.up * verticalOffset * camera.orthographicSize / cameraSizeForOffset;
        Vector2 movement = movementSpeed * Time.deltaTime * direction;
        if (CanMove(movement))
        {
            Vector2 newPosition = (Vector2)transform.position + movement;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    private bool CanMove(Vector2 movement)
    {
        Vector2 actualBottomLeftCorner = camera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 actualBottomRightCorner = camera.ViewportToWorldPoint(new Vector2(1, 0));
        Vector2 actualTopLeftCorner = camera.ViewportToWorldPoint(new Vector2(0, 1));
        Vector2 actualTopRightCorner = camera.ViewportToWorldPoint(new Vector2(1, 1));

        return
            actualBottomLeftCorner.x + movement.x > bottomLeftCorner.position.x &&
            actualBottomRightCorner.x + movement.x < bottomRightCorner.position.x &&
            actualTopLeftCorner.y + movement.y > bottomLeftCorner.position.y &&
            actualTopRightCorner.y + movement.y < topLeftCorner.position.y;
    }
}
