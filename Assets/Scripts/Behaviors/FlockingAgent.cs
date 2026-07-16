using System.ComponentModel;
using UnityEngine;

public class FlockingAgent : MonoBehaviour
{
    [Header("Agent Information")]
    [SerializeField] Vector2 heading;
    [SerializeField] float speed = 10.0f;

    private void Start()
    {
        RandomizeHeading();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += (Vector3)heading * speed * Time.deltaTime;
    }

    private void RandomizeHeading()
    {
        heading = Random.insideUnitCircle.normalized;
    }
}
