using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>Prevents Unity to destroy the game object when the scene changes</para>
/// </summary>
public class CustomDontDestroyOnLoad : MonoBehaviour
{
    //PROBLEM OF PARENTER DONTDESTROYONLOAD OBJECTS
    //When an object becomes a child of a "DontDestroyOnLoad" object, it also became a "DontDestroyOnLoad"..
    //..object. That's makes sense. But it stays as a "DontDestroyOnLoad" object even we make it's parent null..
    //..afterwards. That doesn't make any sense

    private static List<GameObject> dontDestroyOnLoadList;

    private void Awake()
    {
        //??= means if it is null
        dontDestroyOnLoadList ??= new List<GameObject>();
        
        DontDestroyOnLoad(this);
        dontDestroyOnLoadList.Add(gameObject);

        SceneManager.activeSceneChanged += (s, currentScene) => {DestroyObjectsInMainMenu(currentScene);};
    }

    private void DestroyObjectsInMainMenu(Scene currentScene)
    {
        if (currentScene.name != "MAIN_MENU") return;
        
        foreach (GameObject go in dontDestroyOnLoadList)
        {
            Destroy(go);
        }
    }
}