using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManaging;

public class InteractController : MonoBehaviour
{
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

        if (GameState.CurrentState == GameState.States.Playing && !PlayerController.instance.IsMoving)
        {
            Vector3 interactPos = playerController.FacingDir + transform.position;
            Interactible interactCheck = IsInteractible(interactPos);

            Debug.DrawLine(transform.position, interactPos, Color.red, 10);

            if(interactCheck != null)
            {
                Debug.Log("Interacting with " + interactCheck);
                interactCheck.Interact();
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
