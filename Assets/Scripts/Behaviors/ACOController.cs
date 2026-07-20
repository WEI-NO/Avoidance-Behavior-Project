using UnityEngine;

public class ACOController : MonoBehaviour
{
    public static ACOController Instance;
    WorldBorder world;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        world = WorldBorder.Instance;    
    }

    private void Update()
    {
        
    }


}
