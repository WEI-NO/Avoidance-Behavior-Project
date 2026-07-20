using UnityEngine;
using UnityEngine.InputSystem;

public class ImpulseSimulation : Simulator
{
    public bool Enable = true;

    [Header("Click Settings")]
    [SerializeField] float click_radius = 2.0f;
    [SerializeField] float flock_influence_strength = 1.0f;
    [SerializeField] LayerMask flock_layer_mask;

    [Header("Spawn Settings")]
    [SerializeField] FlockingAgent flock_agent_prefab;
    [SerializeField] float flock_spawn_cooldown = 0.5f;
    float flock_spawn_timer = 0.0f;
    

    [Header("Debug")]
    public bool show_debug = false;
    [SerializeField] bool mouse_left_down = false;
    [SerializeField] bool mouse_right_down = false;

    protected override void UpdateSimulation()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            flock_spawn_timer += Time.deltaTime;
            if (flock_spawn_timer >= flock_spawn_cooldown)
            {
                Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mouse_position.z = 0.0f;
                Instantiate(flock_agent_prefab, mouse_position, Quaternion.identity);
                flock_spawn_timer = 0.0f;
            }
            mouse_right_down = true;
        }
        else
        {
            flock_spawn_timer = flock_spawn_cooldown;
            mouse_right_down = false;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouse_position.z = 0.0f;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mouse_position, click_radius, flock_layer_mask);
            foreach (var collider in colliders)
            {
                FlockingAgent agent = collider.GetComponentInParent<FlockingAgent>();
                if (agent != null)
                {
                    var influence_direction = (agent.transform.position - mouse_position).normalized;
                    agent.ApplyExternalInfluence(influence_direction, flock_influence_strength);
                }
                else
                {
                    print("Hit: " + collider.name + " but FlockingAgent is missing");
                }
            }

            mouse_left_down = true;
        }
        else
        {
            mouse_left_down = false;
        }
    }
    protected override void FixedUpdateSimulation() { }

    private void OnDrawGizmos()
    {
        if (show_debug)
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouse_position.z = 0.0f;
            if (mouse_left_down)
            {
                Gizmos.color = Color.green;
            } else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere(mouse_position, click_radius);

            if (mouse_right_down)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(mouse_position, click_radius / 2.0f);
            }
        }
    }
}
