using System.Collections;
using UnityEngine;
using static GameManaging;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement Settings")]
    [SerializeField] float speed;
    [SerializeField] LayerMask objectLayer;

    InputManager input;
    Vector2 inputAxis;

    public bool IsMoving { get; private set; }

    public Vector3 FacingDir { get; private set; }

    private void Awake()
    {
        // creates a single instance of PlayerController
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Destroying " + gameObject.name + "'s PlayerController script");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    
    private void Start()
    {
        input = InputManager.instance;
        IsMoving = false;
    }

    private void Update()
    {
        if(GameState.IsState(GameState.States.Playing))
        {
            inputAxis = input.MoveAxis;

            if (inputAxis.sqrMagnitude > 0 && inputAxis.sqrMagnitude <= 1)
            {
                FacingDir = inputAxis;
                Vector3 newPosition = transform.position + FacingDir;

                if (!IsMoving && CanMove(newPosition))
                {
                    StartCoroutine(Move(newPosition));
                }
            }
        }
    }

    private bool CanMove(Vector3 targetPosition)
    {
        return !Physics2D.OverlapCircle(targetPosition, 0.4f, objectLayer);
    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        IsMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.smoothDeltaTime);

            yield return new WaitForEndOfFrame();
        }

        IsMoving = false;
        transform.position = targetPosition;
    }
}
