using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static GameManaging.Room;

public class DoorInteract : MonoBehaviour, Interactible
{
    [SerializeField]
    public UnityEvent onInteractFinish { get; set; }

    [SerializeField]
    private string roomName;
    [SerializeField]
    private Vector2 roomStartPos;

    public void Interact()
    {
        StartCoroutine(ChangeRoom(roomName, roomStartPos));
    }
}
