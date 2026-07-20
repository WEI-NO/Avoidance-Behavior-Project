using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Simulator : MonoBehaviour
{
    public bool Enabled { get; private set; } = true;

    protected virtual void OnStart() { }
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }

    protected abstract void UpdateSimulation();
    protected abstract void FixedUpdateSimulation();

    private Keyboard keyboard;
    private Mouse mouse;

    protected Keyboard GetKeyboard()
    {
        if (keyboard == null) keyboard = Keyboard.current;
        return keyboard;
    }

    protected Mouse GetMouse()
    {
        if (mouse == null) mouse = Mouse.current;
        return mouse;
    }   

    public void SetEnable()
    {
        Enabled = true;
    }
    
    public void SetDisable()
    {
        Enabled = false;
    }

    private void Start()
    {
        OnStart();
        keyboard = Keyboard.current;
        mouse = Mouse.current;
    }

    private void Awake()
    {
        OnAwake();
    }

    void Update()
    {
        OnUpdate();
        if (Enabled)
            UpdateSimulation();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
        if (Enabled)
            FixedUpdateSimulation();
    }
}
