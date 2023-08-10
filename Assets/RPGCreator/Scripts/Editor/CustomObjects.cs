using UnityEngine;
using UnityEditor;

public static class CustomObjects
{
    private static string prefabPath = "Prefabs/"; // Where the default prefabs are located

    [MenuItem("GameObject/RPG/Interactibles/Dialogue", priority = 2)]
    public static void CreateDialogueInteract(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab(prefabPath + "Dialogue", "Dialogue Interactible");
    }

    [MenuItem("GameObject/RPG/Interactibles/Door", priority = 2)]
    public static void CreateDoorInteract(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab(prefabPath + "Door", "Door Interactible");
    }

    [MenuItem("GameObject/RPG/Room", priority = 2)]
    public static void CreateRoom(MenuCommand menuCommand)
    {
        CreateUtility.CreateScene(prefabPath + "Room", "Room");
    }
}
