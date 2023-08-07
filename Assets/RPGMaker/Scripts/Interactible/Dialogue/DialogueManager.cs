using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

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

    //[HideInInspector]
    public bool inDialogue = false;

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
}