using UnityEngine;
using UnityEditor;

public static class CustomObjects
{
    private static string prefabPath = "Prefabs/"; // Where the default prefabs are located

    [MenuItem("GameObject/RPG/Interactibles/Dialogue")]
    public static void CreateDialogueInteract(MenuCommand menuCommand)
    {
        CreateUtility.CreatePrefab(prefabPath + "Dialogue", "Dialogue Interactible");
    }
}
