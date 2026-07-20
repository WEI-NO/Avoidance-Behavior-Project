using UnityEngine;

public class WorldBorder : MonoBehaviour
{
    public static WorldBorder Instance;

    [Header("World Border Information")]
    [SerializeField] int width = 100;
    [SerializeField] int height = 100;

    [SerializeField] Vector2 map_offset = Vector2.zero;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, 0));
    }

    public Vector2 GetWorldDimension()
    {
        return new Vector2(width, height);
    }
    
    public Vector2 GetMapOffset()
    {
        return map_offset;
    }

    public Vector2 WrapPosition(Vector2 position)
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        position.x = Mathf.Repeat(position.x + halfWidth, width) - halfWidth;
        position.y = Mathf.Repeat(position.y + halfHeight, height) - halfHeight;

        return position;
    }

}
