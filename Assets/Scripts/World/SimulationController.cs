using System;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public enum SimulationMode
    {
        Select,
        Impulse,
        Food,
        ModeCount
    }

    [SerializeField]
    Simulator[] simulators = new Simulator[(int)SimulationMode.ModeCount];

    [SerializeField] SimulationMode defaultMode = SimulationMode.Select;
    [SerializeField] SimulationMode currentMode;

    private void Start()
    {
        FindSimulators();
        SetSimulationMode(defaultMode);
    }

    private void FindSimulators()
    {
        ImpulseSimulation mouseSim = GetComponentInChildren<ImpulseSimulation>();
        if (mouseSim != null)
        {
            simulators[(int)SimulationMode.Impulse] = mouseSim;
        } else
        {
            Debug.LogWarning("Impulse Simulation component missing from SimulationController");
        }


        SelectSimulator selectSim = GetComponentInChildren<SelectSimulator>();
        if (selectSim != null)
        {
            simulators[(int)SimulationMode.Select] = selectSim;
        }
        else
        {
            Debug.LogWarning("Select Simulator component missing from SimulationController");
        }

        FoodSimulator foodSim = GetComponentInChildren<FoodSimulator>();
        if (foodSim != null)
        {
            simulators[(int)SimulationMode.Food] = foodSim;
        }
        else
        {
            Debug.LogWarning("Food Simulator component missing from SimulationController");
        }
    }

    public void SetSimulationMode(SimulationMode mode)
    {
        currentMode = mode;
        for (int i = 0; i < (int)SimulationMode.ModeCount; i++)
        {
            if (simulators[i] == null) continue;
            if (i == (int)mode)
            {
                simulators[i].SetEnable();
            } else
            {
                simulators[i].SetDisable();
            }
        }
    }

    public void SetSimulationMode(int mode)
    {
        SimulationMode m = (SimulationMode)mode;

        SetSimulationMode(m);
    }



}
