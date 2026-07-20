using System.Collections;
using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Food Settings")]
    public LayerMask foodLayer;
    [SerializeField] float foodDetectionCooldown = 1.0f;
    [SerializeField] float foodDetectionRadius = 1.0f;

    [SerializeField] GameObject foodTarget;
    [SerializeField] float consumeDistance = 0.5f;

    public bool hungry = true;

    [Header("Agent Settings")]
    [SerializeField] Agent2D agent;

    private void Awake()
    {
        agent = GetComponent<Agent2D>();
        if (agent == null)
        {
            Debug.LogWarning("Agent2D component missing");
        }
    }

    private void Start()
    {
        StartCoroutine(FoodDetection());
    }

    private void Update()
    {
        if (foodTarget != null)
        {
            agent.simulateAgent = false;
            var dist = Vector2.Distance((Vector2)foodTarget.transform.position, transform.position);
            if (dist <= consumeDistance)
            {
                Destroy(foodTarget);
            }
        } else
        {
            agent.simulateAgent = true;
        }
    }

    IEnumerator FoodDetection()
    {
        while (true)
        {
            yield return new WaitForSeconds(foodDetectionCooldown);

            if (hungry)
                DetectFood();
        }

    }

    private void DetectFood()
    {
        if (foodTarget != null)
        {
            return;
        }
        var colliders = Physics2D.OverlapCircleAll(transform.position, foodDetectionRadius, foodLayer);
        foreach (var c in colliders)
        {
            var treat = c.GetComponentInParent<FishTreat>();
            if (treat != null)
            {
                if (treat.Reserve(this))
                {
                    foodTarget = treat.gameObject;
                    break;
                } else
                {
                    continue;
                }
            }
        }

        if (agent != null && foodTarget != null)
        {
            agent.SetDestination(foodTarget.transform.position);
            agent.SetDestinationEnable(true);
        }
        else
        {
            agent.SetDestinationEnable(false);
        }
    }
}
