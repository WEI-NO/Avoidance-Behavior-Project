using UnityEngine;

public enum AgentType
{
    FlockingAgent,
    Count
}

public abstract class Agent2D : MonoBehaviour
{
    [Header("Agent Information")]
    public string AgentName = "Agent";
    public AgentType agentType { get; protected set; } = AgentType.FlockingAgent;
    protected Rigidbody2D rb;

    [Header("Movement Settings")]
    public Vector2 Heading;
    public float Speed = 10.0f;
    private Vector2 prevHeading;
    public bool simulateAgent = true;

    [Header("Visual Settings")]
    [SerializeField] protected bool Enable_RotateToHeading = true;
    [SerializeField] protected bool Enable_Animation = true;

    [Header("Targetting Settings")]
    [SerializeField] bool enableDestination = false;
    public Vector2 destination;
    public float destinationAttractionRate = 0.5f;

    protected virtual void MoveAgent()
    {
        if (Heading.magnitude < 1.0f)
        {
            Heading = Heading.normalized;
        }
        rb.linearVelocity = Heading * Speed * Time.deltaTime;
        transform.position = WorldBorder.Instance.WrapPosition(transform.position);
    }

    protected void NudgeDirection()
    {
        if (enableDestination == false) return;
        var heading = Heading;

        var magnitude = heading.magnitude;
        var directionToFood = (destination - (Vector2)transform.position).normalized;
        var newHeading = Vector2.Lerp(heading.normalized, directionToFood, destinationAttractionRate * Time.deltaTime);
        Heading = newHeading * magnitude;
    }

    private void RotateToHeading()
    {
        if (!Enable_RotateToHeading) return;

        // Rotate the agent to face the Heading direction
        float angle = Mathf.Atan2(Heading.y, Heading.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected abstract void UpdateAgent();
    protected virtual void CleanupUpdate() { }
    protected virtual void OnStart() { }
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }

    private void Start()
    {
        OnStart();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        OnAwake();
    }

    void Update()
    {
        OnUpdate();
        if (simulateAgent)
            UpdateAgent();
        CleanupUpdate();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
        NudgeDirection();
        MoveAgent();
        RotateToHeading();
    }

    public float GetAbsoluteSpeed()
    {
        return Heading.magnitude * Speed * Time.deltaTime;
    }

    public void SetDestination(Vector2 destination)
    {
        this.destination = destination;
    }

    public void SetDestinationEnable(bool enable)
    {
        enableDestination = enable;
    }
}
