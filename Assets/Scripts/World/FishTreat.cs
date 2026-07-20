using UnityEngine;

public class FishTreat : MonoBehaviour
{
    [SerializeField] float minSize = 0.3f;
    [SerializeField] Vector2 maxSize = new Vector2(1.5f, 2.0f);
    [SerializeField] float lifeTime = 1.0f;
    [SerializeField] float lifeTimer = 0.0f;
    float chosenMaxSize = 1.0f;

    public Fish reservedFish;

    private void Start()
    {
        lifeTimer = lifeTime;
        chosenMaxSize = Random.Range(maxSize.x, maxSize.y);
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0.0f)
        {
            Destroy(gameObject);
        }
        ScaleByLifeTime();
    }

    private void ScaleByLifeTime()
    {
        var progress = lifeTimer / lifeTime;
        var size = Mathf.Lerp(minSize, chosenMaxSize, progress);
        transform.localScale = new Vector3(size, size, 1.0f);
    }

    public bool Reserve(Fish fish)
    {
        // Fish is invalid
        if (fish == null)
            return false;
        // Same fish reserving food
        if (reservedFish != null && fish == reservedFish) return true;

        // Fish reserves a reserved food
        if (reservedFish != null)
        {
            return false;
        }

        // Reserve food
        reservedFish = fish;
        return true;
    }
}
