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

    private DialogueManager dialogueManager;

    private Animator animator;

    private TextMeshProUGUI text;
    private TextMeshProUGUI nameText;
    private Image image;
    private GameObject textBox;
    private GameObject nameTextBox;
    private GameObject imageBox;

    private float textDelay;

    // this function will be activated from other scripts
    public void Interact()
    {
        if (switchEnable.Length > 0 && SwitchManager.instance.GetSwitchState(switchEnable))
        {
            Debug.Log("Starting dialogue '" + transform.name + "'.");
            StartDialogue();
        }
    }

    // used as a way to setup the dialogue
    private void StartDialogue()
    {
        dialogueManager = DialogueManager.instance;
        dialogueManager.inDialogue = true;

        // starts the dialogueIn animation
        animator = dialogueManager.animator;

        animator.SetBool("Active", true);

        // fetches all of the needed variables from DialogueManager
        if (dialogueLines != null)
        {
            // gets all the necessary variables from the DialogueManager
            text = dialogueManager.text;
            nameText = dialogueManager.nameText;
            image = dialogueManager.image;
            textBox = dialogueManager.textBox;
            nameTextBox = dialogueManager.nameBox;
            imageBox = dialogueManager.imageBox;

            textDelay = dialogueManager.textDelay;

            StartCoroutine(NextLine(0));
        }
        else
        {
            Debug.LogWarning(transform.name + " has no dialogue lines to display");
        }
    }

    private IEnumerator NextLine(int lineIndex)
    {
        DialogueLine line = dialogueLines[lineIndex];

        text.text = line.dialogue;
        text.maxVisibleCharacters = 0;

        // hides the name box depending on whether line.name is empty or not
        if (line.name.Length > 0)
        {
            nameTextBox.SetActive(true);
            nameText.text = line.name;
        }
        else
        {
            nameTextBox.SetActive(false);
        }

        // hides the image box depending on whether line.image is null or not
        if (line.image)
        {
            imageBox.SetActive(true);
            image.sprite = line.image;
        }
        else
        {
            imageBox.SetActive(false);
        }

        // adds one to maxVisibleCharacters until it the text is fully visible
        // each loop is delayed by textDelay
        for (int charIndex = 0; charIndex <= line.dialogue.Length; charIndex++)
        {
            text.maxVisibleCharacters = charIndex;

            yield return new WaitForSeconds(textDelay);
        }

        // waits until the specified key is pressed
        var trigger = false;
        UnityEvent interact = InputManager.instance.interact;
        UnityAction action = () => trigger = true;

        interact.AddListener(action.Invoke);
        yield return new WaitUntil(() => trigger);
        interact.RemoveListener(action.Invoke);

        if (dialogueLines.Length > lineIndex + 1)
        {
            StartCoroutine(NextLine(lineIndex + 1));
        }
        else
        {
            // plays the dialogueOut animation and delays the script by the animation clip length
            float animationDelay = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            animator.SetBool("Active", false);

            yield return new WaitForSeconds(animationDelay);

            // invokes the on finish event
            onInteractFinish.Invoke();

            // resets the texts and image to be empty
            dialogueManager.inDialogue = false;
            text.text = "";
            nameText.text = "";
            image.sprite = null;
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