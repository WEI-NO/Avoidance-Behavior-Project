using TMPro;
using UnityEngine;

public class InformationPanel : MonoBehaviour
{
    public static InformationPanel Instance;
    [Header("References")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI agentTypeText;
    [SerializeField] GameObject content;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateInformation(Agent2D agent)
    {
        if (agent == null)
        {
            SetDisable();
            return;
        }
        nameText.text = agent.AgentName;
        agentTypeText.text = agent.agentType.ToString();
        SetEnable();
    }
    
    public void SetEnable()
    {
        content.SetActive(true);
    }

    public void SetDisable()
    {
        content.SetActive(false);
    }

}
