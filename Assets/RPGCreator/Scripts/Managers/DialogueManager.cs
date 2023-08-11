using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using static GameManaging;
using Unity.VisualScripting;
using System.Collections.Generic;

// this script is used as a manager for dialogue variables
//[ExecuteInEditMode]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue")]
    public TextMeshProUGUI text;
    public TextMeshProUGUI nameText;
    public Image image;
    [Space]
    public GameObject textBox;
    public GameObject nameBox;
    public GameObject imageBox;
    [Space]
    public float textDelay;

    [Header("Animation")]
    [Tooltip("If no Animator is specified, no animation will be played")]
    public Animator animator;

    private string prevLineName;

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
}