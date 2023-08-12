using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

using static GameManaging.GameState;
using UnityEngine.Events;

/// <summary>
/// This script is used as a manager for playing dialogue.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue")]
    public TextMeshProUGUI text;
    public TextMeshProUGUI speaker;
    public Image image;
    [Space]
    public GameObject textBox;
    public GameObject speakerBox;
    public GameObject imageBox;
    [Space]
    public float textDelay = 0.3f;

    [Header("Animation")]
    [Tooltip("If no Animator is specified, no animation will be played")]
    public Animator animator;

    private void Awake()
    {
        // creates a single instance of DialogueManager
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Destroying " + gameObject.name + "'s DialogueManager script");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public static void StartDialogue(DialogueLine[] dialogueLines)//, params string[] variables)
    {
        if(dialogueLines.Length > 0)
        {
            ChangeState(States.Busy);
            if (instance.animator != null) instance.animator.SetBool("Active", true);

            instance.StartCoroutine(instance.NextLine(dialogueLines));
        }
        else
        {
            Debug.LogWarning("Missing dialogue lines!");
        }
    }

    public IEnumerator NextLine(DialogueLine[] lines, int lineIndex = 0)
    {
        UnityEvent interactEvent = InputManager.instance.interact;
        DialogueLine currentLine = lines[lineIndex];

        bool mustSkip = false;
        UnityAction skipTrigger = () => mustSkip = true;
        interactEvent.AddListener(skipTrigger.Invoke);

        text.text = currentLine.dialogue;
        text.maxVisibleCharacters = 0;

        // if the dialogue line has an image or not
        // set the image.sprite to the line's image
        if (currentLine.image)
        {
            imageBox.SetActive(true);
            image.sprite = currentLine.image;
        }
        else imageBox.SetActive(false);

        // if the dialogue line has a speaker or not
        // set the speaker.text to the line's speaker
        if (currentLine.speaker.Length > 0)
        {
            speakerBox.SetActive(true);
            speaker.text = currentLine.speaker;
        }
        else speakerBox.SetActive(false);

        // gives the dialogue a typing effect
        for (int charIndex = 0; charIndex <= currentLine.dialogue.Length; charIndex++)
        {
            text.maxVisibleCharacters += charIndex;

            if (mustSkip && charIndex > 5)
            {
                text.maxVisibleCharacters = currentLine.dialogue.Length;
                break;
            }

            yield return new WaitForSecondsRealtime(textDelay);
        }

        mustSkip = false;
        interactEvent.RemoveListener(skipTrigger.Invoke);

        // events for checking if the player has pressed the interact button
        bool trigger = false;
        UnityAction activateTrigger = () => trigger = true;

        // waiting for the player to press the interact button
        interactEvent.AddListener(activateTrigger.Invoke);
        yield return new WaitUntil(() => trigger);
        interactEvent.RemoveListener(activateTrigger.Invoke);

        if (lineIndex + 1 < lines.Length)
        {
            StartCoroutine(NextLine(lines, ++lineIndex));
            yield break;
        }
        else
        {
            if(animator != null) animator.SetBool("Active", false);

            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationLength);

            ChangeState(States.Playing);
        }
    }

    // used to easily manage dialogue lines
    [System.Serializable]
    public struct DialogueLine
    {
        public string speaker;
        public string dialogue;
        public Sprite image;
    }
}


/*
    // used as a way to setup the dialogue
    public static void StartDialogue(
        DialogueInteract.DialogueLine[] dialogueLines,
        DialogueInteract dialogueInteract = null)
    {
        // fetches all of the needed variables from DialogueManager
        if (dialogueLines.Length > 0)
        {
            GameState.ChangeState(GameState.States.Busy);

            // starts the dialogueIn animation
            instance.animator.SetBool("Active", true);

            if (!dialogueInteract)
            {
                instance.StartCoroutine(NextLine(0, dialogueLines[0]));
            }
            else
            {
                instance.StartCoroutine(NextLine(0, dialogueLines[0], dialogueInteract));
            }
        }
        else
        {
            Debug.LogError("No dialogue lines to display!");
        }
    }

    private static IEnumerator NextLine(int lineIndex, DialogueInteract.DialogueLine line, DialogueInteract dialogueInteract = null)
    {
        // the current line of dialogue
        //DialogueInteract.DialogueLine line = dialogueInteract.dialogueLines[lineIndex];
        
        bool mustSkip = false;
        InputManager.instance.interact.AddListener(() =>
        {
            mustSkip = true;
        });

        instance.text.text = line.dialogue;
        instance.text.maxVisibleCharacters = 0;

        // hides the name box depending on whether line.name is empty or not
        if (line.name.Length > 0)
        {
            instance.nameBox.SetActive(true);
            instance.nameText.text = line.name;
        }
        else
        {
            instance.nameBox.SetActive(false);
        }

        // hides the image box depending on whether line.image is null or not
        if (line.image)
        {
            instance.imageBox.SetActive(true);
            instance.image.sprite = line.image;
        }
        else
        {
            instance.imageBox.SetActive(false);
        }

        // adds one to maxVisibleCharacters until it the text is fully visible
        // each loop is delayed by textDelay
        for (int charIndex = 0; charIndex <= line.dialogue.Length; charIndex++)
        {
            instance.text.maxVisibleCharacters = charIndex;

            // if the player presses the interact key
            // during dialogue, skip the dialogue
            if (mustSkip) break;

            yield return new WaitForSeconds(instance.textDelay);
        }

        mustSkip = false;
        instance.text.maxVisibleCharacters = instance.text.text.Length;

        // events for checking if the player has pressed the interact button
        var trigger = false;
        UnityEvent interact = InputManager.instance.interact;
        UnityAction action = () => trigger = true;

        // waiting for the player to press the interact button
        interact.AddListener(action.Invoke);
        yield return new WaitUntil(() => trigger);
        interact.RemoveListener(action.Invoke);

        if (dialogueInteract && dialogueInteract.dialogueLines.Length > lineIndex + 1)
        {
            instance.StartCoroutine(NextLine(
                lineIndex + 1, dialogueInteract.dialogueLines[lineIndex + 1], dialogueInteract
            ));
        }
        else
        {
            // plays the dialogueOut animation and delays the script by the animation clip length
            float animationDelay = instance.animator.GetCurrentAnimatorStateInfo(0).length;
            instance.animator.SetBool("Active", false);

            yield return new WaitForSeconds(animationDelay);

            // invokes the on finish event
            //dialogueInteract.onInteractFinish.Invoke();

            // change the game state back to playing
            GameState.ChangeState(GameState.States.Playing);

            // resets the texts and image to be empty
            instance.text.text = "";
            instance.nameText.text = "";
            instance.image.sprite = null;
        }
    }
*/