using UnityEngine;

public class FoodSimulator : Simulator
{
    public float heldCooldown = 0.25f;
    float heldTimer = 0.0f;
    public Vector2Int spawnAmountRange = new Vector2Int(2, 5);
    public float spawnRadius = 1.0f;

    [SerializeField] FishTreat treat;

    protected override void FixedUpdateSimulation()
    {

    }

    protected override void UpdateSimulation()
    {
        if (GetMouse().leftButton.isPressed)
        {
            heldTimer -= Time.deltaTime;
            if (heldTimer <= 0.0f)
            {
                SpawnTreat();
                heldTimer = heldCooldown;
            }
        }
    }

    private void SpawnTreat()
    {
        int spawnAmount = Random.Range(spawnAmountRange.x, spawnAmountRange.y);
        
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            var worldPos = Camera.main.ScreenToWorldPoint(GetMouse().position.ReadValue());
            Vector2 spawnPos = (Vector2)worldPos + randomPoint;
            FishTreat t = Instantiate(treat, new Vector3(spawnPos.x, spawnPos.y, 0.0f), Quaternion.Euler(0, 0, Random.Range(0, 360.0f)), transform);
        }
    }
}
