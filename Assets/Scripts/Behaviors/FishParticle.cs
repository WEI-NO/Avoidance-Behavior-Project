using UnityEngine;

public class FishParticle : MonoBehaviour
{
    [Header("Particle Settings")]
    public int minEmissionRate = 2;
    public int maxEmissionRate = 10;
    public float maxSpeed = 10.0f;


    [SerializeField] Agent2D target;
    [SerializeField] ParticleSystem particle;

    private void Awake()
    {
        target = GetComponent<Agent2D>();
        if (target == null)
            Debug.LogWarning("Agent2D component is missing from parent");

        particle = GetComponentInChildren<ParticleSystem>();
        if (particle == null)
            Debug.LogWarning("ParticleSystem component is missing");
    }

    private void Update()
    {
        UpdateParticleSystem();
    }

    private void UpdateParticleSystem()
    {
        if (target == null || particle == null) return;

        var speed = target.GetAbsoluteSpeed();

        var speedRate = speed / maxSpeed;
        var emissionRate = Mathf.RoundToInt(Mathf.Lerp(minEmissionRate, maxEmissionRate, speedRate));

        
    }

}
