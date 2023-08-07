using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of Input Manager.
    /// </summary>
    public static InputManager instance;

    /// <summary>
    /// C# class for the user's input.
    /// </summary>
    public DefaultInput inputs;

    /// <summary>
    /// Input axis for WASD and Arrow Keys.
    /// Updated every frame to match the user input.
    /// </summary>
    [HideInInspector]
    public Vector2 moveAxis { get; private set; }

    [Header("Input Events")]
    /// <summary>
    /// Input event for the Interact key.
    /// Will be invoked when the user presses the Interact key.
    /// </summary>
    public UnityEvent interact;

    /// <summary>
    /// Input event for the Back key.
    /// Will be invoked when the user presses the Back key.
    /// </summary>
    public UnityEvent back;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Destroying " + gameObject.name + "'s InputManager script");
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        inputs = new DefaultInput();
    }

    private void Update()
    {
        inputs.Player.Interact.performed += _ => interact.Invoke();
        inputs.Player.Back.performed += _ => back.Invoke();

        moveAxis = inputs.Player.Move.ReadValue<Vector2>();
    }

    private void OnEnable() { inputs.Enable(); }

    private void OnDisable() { inputs?.Disable(); }
}
