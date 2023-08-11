using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueInteract : MonoBehaviour, Interactible
{
    public DialogueLine[] dialogueLines;
    public UnityEvent onInteractFinish { get; set; }
    [SerializeField] string switchEnable;

    // this function can/will be activated from other scripts
    public void Interact()
    {
        if (switchEnable.Length > 0 && SwitchManager.instance.GetSwitchState(switchEnable))
        {
            Debug.Log("Starting dialogue '" + transform.name + "'.");
            DialogueManager.StartDialogue(dialogueLines, this);
        }
    }

    // used to easily manage dialogue lines
    [System.Serializable]
    public class DialogueLine
    {
        public string name;
        public string dialogue;
        public Sprite image;
    }
}