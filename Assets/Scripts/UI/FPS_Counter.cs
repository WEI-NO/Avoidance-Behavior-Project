using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;

    private void Update()
    {
        if (fpsText == null) return;
        float fps = 1 / Time.deltaTime;
        fpsText.text = $"FPS: {(int)fps}";
    }
}
