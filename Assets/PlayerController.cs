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
    bool isMoving = false;
    readonly bool canMove;

    public Vector3 facingDir;

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
    }

    private void Update()
    {
        if(GameState.IsState(GameState.GameStates.Playing))
        {
            inputAxis = input.moveAxis;

            if (inputAxis.sqrMagnitude > 0 && inputAxis.sqrMagnitude <= 1)
            {
                facingDir = inputAxis;
                Vector3 newPosition = transform.position + facingDir;

                if (!isMoving && CanMove(newPosition))
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
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            yield return null;
        }

        isMoving = false;
        transform.position = targetPosition;
    }
}
