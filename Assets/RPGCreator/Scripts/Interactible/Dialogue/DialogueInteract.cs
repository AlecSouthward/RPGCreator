using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueInteract : MonoBehaviour, Interactible
{
    public DialogueManager.DialogueLine[] dialogueLines;
    public UnityEvent onInteractFinish { get; set; }
    [SerializeField] string switchEnable;

    // this function can/will be activated from other scripts
    public void Interact()
    {
        if (switchEnable.Length > 0 && SwitchManager.instance.GetSwitchState(switchEnable))
        {
            DialogueManager.StartDialogue(dialogueLines);
        }
    }
}