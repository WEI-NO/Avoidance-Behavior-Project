using UnityEngine;
using UnityEngine.InputSystem;

public class SelectSimulator : Simulator
{
    [SerializeField] GameObject prefabSelectionRing;
    private GameObject currentSelectionRing;
    private Agent2D currentSelectedAgent;
    private bool notifiedCamera = false;

    protected override void FixedUpdateSimulation()
    {
        UpdateSelectionRing();
    }

    protected override void UpdateSimulation()
    {
        if (!notifiedCamera)
        {
            CameraSimulation.Instance.SetTarget(currentSelectedAgent);
            notifiedCamera = true;
        }

        if (GetMouse().leftButton.wasPressedThisFrame)
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(GetMouse().position.ReadValue());
            mouse_position.z = 0.0f;
            Collider2D collider = Physics2D.OverlapCircle(mouse_position, 0.1f);
            if (collider != null)
            {
                FlockingAgent agent = collider.GetComponentInParent<FlockingAgent>();
                if (agent != null)
                {
                    InformationPanel.Instance.UpdateInformation(agent);
                    CreateSelectionRingOn(agent);
                    currentSelectedAgent = agent;
                    notifiedCamera = false;
                    return;
                }
                else
                {
                    print("Hit: " + collider.name + " but FlockingAgent is missing");
                }
            }
            InformationPanel.Instance.UpdateInformation(null);
            CreateSelectionRingOn(null);
            currentSelectedAgent = null;
            notifiedCamera = false;
        }
    }

    private void CreateSelectionRingOn(Agent2D agent)
    {
        if (currentSelectionRing != null)
        {
            Destroy(currentSelectionRing);
            currentSelectionRing = null;
        }
        if (agent == null)
        {
            return;
        }

        var newRing = Instantiate(prefabSelectionRing, transform, true);
        currentSelectionRing = newRing;
        currentSelectionRing.transform.position = agent.transform.position;
    }

    private void UpdateSelectionRing()
    {
        if (currentSelectionRing == null || currentSelectedAgent == null) return;

        var agent_position = currentSelectedAgent.transform.position;
        currentSelectionRing.transform.position = agent_position;
    }

}
