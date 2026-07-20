using JetBrains.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FlockingAgent : Agent2D
{

    [SerializeField] LayerMask detect_layer;

    [Header("Flocking Information")]
    [SerializeField] float close_radius = 1.0f;
    [SerializeField] float far_radius = 1.0f; // Heading

    [SerializeField] float separation_weight = 1.0f;
    [SerializeField] float alignment_weight = 1.0f;
    [SerializeField] float cohesion_weight = 1.0f;

    [SerializeField] float turn_speed = 1.0f;

    [Header("Control Settings")]
    public bool apply_constant_rotation = false;
    [SerializeField] float constant_rotation = 1.0f;

    public bool apply_random_movement = false;
    [SerializeField] float random_movement_interval = 1.0f;
    float random_movement_timer = 0.0f;
    [SerializeField] Vector2 random_movement_range = new Vector2(-1.0f, 1.0f);
    [SerializeField] float random_movement_strength = 1.0f;


    [SerializeField] List<FlockingAgent> neighbors_close = new List<FlockingAgent>();
    [SerializeField] List<FlockingAgent> neighbors_far = new List<FlockingAgent>();

    [Header("Influence Settings")]
    // External Influence
    [SerializeField] Vector2 external_influence = Vector2.zero;
    [SerializeField] float external_influence_decay_rate = 1.0f;

    [Header("Debug")]
    public bool show_debug = false;

    protected override void OnStart()
    {
        RandomizeHeading();
    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void UpdateAgent()
    {
        GetNeighbors();

        var separation = CalculateSeparation();
        var alignment = CalculateAlignment();
        var cohesion = CalculateCohesion();

        var direction =
            separation * separation_weight +
            alignment * alignment_weight +
            cohesion * cohesion_weight;

        direction.Normalize();

        Vector2 newHeading = Vector2.Lerp(Heading, direction, turn_speed * Time.fixedDeltaTime);

        Heading = newHeading.normalized;

        // Random Movement
        if (apply_random_movement)
        {
            random_movement_timer += Time.fixedDeltaTime;
            if (random_movement_timer >= random_movement_interval)
            {
                random_movement_timer = 0.0f;
                Heading = Vector2.Lerp(Heading, new Vector2(
                    Random.Range(random_movement_range.x, random_movement_range.y),
                    Random.Range(random_movement_range.x, random_movement_range.y)
                ), random_movement_strength * Time.fixedDeltaTime);
            }
        }

        // Constant Rotation
        if (apply_constant_rotation)
        {
            float angle = constant_rotation * Time.fixedDeltaTime;
            Heading = Quaternion.Euler(0, 0, angle) * Heading;
        }
    }

    protected override void CleanupUpdate()
    {
        // External Influence
        if (external_influence != Vector2.zero)
        {
            Heading += external_influence * Time.deltaTime;
            external_influence = Vector2.Lerp(external_influence, Vector2.zero, external_influence_decay_rate * Time.deltaTime);
            if (external_influence.magnitude <= .01f)
            {
                external_influence = Vector2.zero;
            }
        }
    }

    private void RandomizeHeading()
    {
        Heading = Random.insideUnitCircle.normalized;
    }

    private Vector2 CalculateCohesion() // Heading
    {
        // Get average position of all neighbors
        Vector2 averagePos = Vector2.zero;

        foreach (var n in neighbors_far)
        {
            averagePos += (Vector2)n.transform.position;
        }

        Vector2 cohesion = Vector2.zero;

        if (neighbors_far.Count == 0) return cohesion;

        averagePos /= neighbors_far.Count;
        cohesion = averagePos - (Vector2)transform.position;
        return cohesion;
    }

    private Vector2 CalculateAlignment()
    {
        Vector2 alignment = Vector2.zero;
        foreach (var n in neighbors_far)
        {
            alignment += n.Heading;
        }

        if (neighbors_far.Count == 0) return alignment;
        alignment /= neighbors_far.Count;
        return alignment;
    }

    private Vector2 CalculateSeparation()
    {
        Vector2 separation = Vector2.zero;

        foreach (var n in neighbors_close)
        {
            Vector2 directionAway = transform.position - n.transform.position;
            float distance = directionAway.magnitude;

            if (distance > 0f)
            {
                separation += directionAway.normalized / distance;
            }
        }

        return separation;
    }

    private void GetNeighbors()
    {
        neighbors_close.Clear();
        neighbors_far.Clear();
        var collisions = Physics2D.OverlapCircleAll(transform.position, far_radius, detect_layer);

        foreach (var c in collisions)
        {
            var agent = c.GetComponentInParent<FlockingAgent>();
            if (agent != null && agent != this)
            {
                neighbors_far.Add(agent);
                var distance = (transform.position - agent.transform.position).magnitude;
                if (distance <= close_radius)
                {
                    neighbors_close.Add(agent);
                }
            }
        }
    }

    public void ApplyExternalInfluence(Vector2 influence_direction, float influence_strength)
    {
        external_influence += influence_direction.normalized * influence_strength;
    }

    private void OnDrawGizmos()
    {
        if (show_debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, close_radius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, far_radius);
        }
    }
}
