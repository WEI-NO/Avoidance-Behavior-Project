using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSimulation : MonoBehaviour
{
    public static CameraSimulation Instance;

    [Header("Move Settings")]
    [SerializeField] float ControlMoveSensitivity = 1.0f;
    [SerializeField] float CameraMoveSpeed = 1.0f;
    [SerializeField] float ControlFalloffRate = 1.0f;
    Vector3 moveDirection = Vector3.zero;
    
    [SerializeField] bool useWorldBound = false;
    private WorldBorder worldBorder;

    [Header("Zoom Settings")]
    [SerializeField] float ZoomSensitivity = 1.0f;
    [SerializeField] Vector2 zoomRange = new Vector2(3.0f, 20.0f);

    [Header("Target Settings")]
    Vector3 destination;
    [SerializeField] private Agent2D targetAgent;

    private Camera cam;
    private Keyboard keyboard;
    private Mouse mouse;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        keyboard = InputSystem.GetDevice<Keyboard>();
        mouse = InputSystem.GetDevice<Mouse>();

        destination = transform.position;
    }

    private void Update()
    {
        Scroll_CameraZoom();
        WASD_Control();
        TargetFollowUpdate();
    }

    private void FixedUpdate()
    {
        WASD_CameraMovement();
        PositionFixedUpdate();
    }

    private void TargetFollowUpdate()
    {
        if (targetAgent == null) return;

        destination = targetAgent.transform.position;
        destination.z = -10;
    }

    private void Scroll_CameraZoom()
    {
        if (mouse.scroll.y.ReadValue() > 0)
        {
            cam.orthographicSize -= ZoomSensitivity * Time.deltaTime;
        }
        else if (mouse.scroll.y.ReadValue() < 0)
        {
            cam.orthographicSize += ZoomSensitivity * Time.deltaTime;
        }
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, zoomRange.x, zoomRange.y);
    }

    private void PositionFixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(destination.x, destination.y, -10), Time.fixedDeltaTime * CameraMoveSpeed);
    }

    private void WASD_Control()
    {
        if (keyboard.wKey.isPressed)
        {
            moveDirection += Vector3.up * ControlMoveSensitivity * Time.deltaTime;
        }

        if (keyboard.sKey.isPressed)
        {
            moveDirection += Vector3.down * ControlMoveSensitivity * Time.deltaTime;
        }

        if (keyboard.aKey.isPressed)
        {
            moveDirection += Vector3.left * ControlMoveSensitivity * Time.deltaTime;
        }

        if (keyboard.dKey.isPressed)
        {
            moveDirection += Vector3.right * ControlMoveSensitivity * Time.deltaTime;
        }

        moveDirection = Vector2.Lerp(moveDirection, Vector2.zero, ControlFalloffRate * Time.deltaTime);
    }

    private void WASD_CameraMovement()
    {
        if (targetAgent != null) return;
        Vector3 newPosition = transform.position + moveDirection * Time.fixedDeltaTime * ControlMoveSensitivity;
        newPosition = ClampToWorldBorder(newPosition);
        destination = newPosition;
    }

    private Vector3 ClampToWorldBorder(Vector3 nextPosition)
    {
        if (!useWorldBound)
            return nextPosition;

        if (worldBorder == null)
            worldBorder = WorldBorder.Instance;

        if (worldBorder == null)
            return nextPosition;

        Camera cam = Camera.main;

        if (cam == null)
            return nextPosition;

        //float cameraHalfHeight = cam.orthographicSize;
        //float cameraHalfWidth = cameraHalfHeight * cam.aspect;

        float worldHalfWidth = worldBorder.GetWorldDimension().x * 0.5f;
        float worldHalfHeight = worldBorder.GetWorldDimension().y * 0.5f;

        //float minX = -worldHalfWidth + cameraHalfWidth;
        //float maxX = worldHalfWidth - cameraHalfWidth;

        //float minY = -worldHalfHeight + cameraHalfHeight;
        //float maxY = worldHalfHeight - cameraHalfHeight;

        //nextPosition.x = minX > maxX
        //    ? 0f
        //    : Mathf.Clamp(nextPosition.x, minX, maxX);

        //nextPosition.y = minY > maxY
        //    ? 0f
        //    : Mathf.Clamp(nextPosition.y, minY, maxY);

        float minX = -worldHalfWidth;
        float maxX = worldHalfWidth;

        float minY = -worldHalfHeight;
        float maxY = worldHalfHeight;

        nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);

        // nextPosition.z is never changed.
        return nextPosition;
    }

    public void SetTarget(Agent2D target)
    {
        if (target == null)
        {
            targetAgent = null;
            return;
        }

        targetAgent = target;
    }
}
