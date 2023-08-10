using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class CreateUtility
{
    public static void CreatePrefab(string path, string name)
    {
        // loads the prefab from the Resources folder        
        GameObject newObject = GameObject.Instantiate(Resources.Load(path)) as GameObject;
        
        newObject.name = name;
        Place(newObject);
    }

    public static void CreateScene(string path, string name)
    {
        //SceneAsset scenePrefab = Resources.Load(path) as SceneAsset;

        // creates a new empty scene (WIP)
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
        newScene.name = name;

        EditorSceneManager.MarkSceneDirty(newScene);
    }

    public static void Place(GameObject gameObject)
    {
        // Find location
        SceneView lastView = SceneView.lastActiveSceneView;
        gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

        // Make sure we place the object in the proper scene, with a relevant name
        StageUtility.PlaceGameObjectInCurrentStage(gameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

        // Record undo, and select
        Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
        Selection.activeGameObject = gameObject;
        

        // For prefabs, let's mark the scene as dirty for saving
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}

/*
GameObject newObject = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
 
public static void CreateObject(string name, params Type[] types)
{
    GameObject newObject = ObjectFactory.CreateGameObject(name, types);
    Place(newObject);
}
*/