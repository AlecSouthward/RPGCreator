using UnityEngine;
using UnityEngine.Events;
using static DialogueManager;
using static InventoryManager;

public class ItemInteract : MonoBehaviour, Interactible
{
    public UnityEvent onInteractFinish { get; set; }

    [SerializeField]
    private Item item;

    [SerializeField]
    private DialogueLine[] dialogue;

    public void Interact()
    {
        // formats every {0} in all the dialogue lines to the item name
        for (int lineIndex = 0; lineIndex < dialogue.Length; lineIndex++)
        {
            DialogueLine line = dialogue[lineIndex];

            line.dialogue = string.Format(line.dialogue, item.name);
        }

        // adds the item to the inventory and then displays dialogue
        InventoryManager.instance.AddItem(item);
        StartDialogue(dialogue);
    }
}
