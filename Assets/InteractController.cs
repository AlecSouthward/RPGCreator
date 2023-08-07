using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour
{
    [SerializeField] DialogueInteract dialogue;
    [SerializeField] bool canInteract = true;

    private PlayerController playerController;
    [SerializeField]
    private LayerMask interactLayer;

    private void Start()
    {
        playerController = PlayerController.instance;

        InputManager.instance.interact.AddListener(() => Interact());
    }

    void Interact()
    {
        //canInteract = DialogueManager.instance.inDialogue == false;

        if (canInteract)
        {
            Vector3 interactPos = (Vector3)playerController.facingDir + transform.position;
            Interactible interactCheck = IsInteractible(interactPos);

            Debug.DrawLine(transform.position, interactPos, Color.red, 10);

            if (interactCheck != null)
            {
                interactCheck.Interact();

                canInteract = false;
                interactCheck.onInteractFinish.AddListener(() => {
                    canInteract = true;
                });
            }
            else
            {
                Debug.Log("Nothing to interact with.");
            }
        }
    }

    Interactible IsInteractible(Vector2 targetPosition)
    {
        Collider2D objCheck = Physics2D.OverlapCircle(targetPosition, 0.4f, interactLayer);

        if (!objCheck) return null;

        objCheck.TryGetComponent<Interactible>(out var objInteractible);

        return objInteractible;
    }
}
