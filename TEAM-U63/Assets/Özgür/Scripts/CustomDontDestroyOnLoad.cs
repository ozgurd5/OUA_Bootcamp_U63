using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Prevents Unity to destroy the game object when the scene changes</para>
/// </summary>
public class CustomDontDestroyOnLoad : MonoBehaviour
{
    public static List<GameObject> DontDestroyOnLoadList;

    private void Awake()
    {
        //??= means if it is null
        DontDestroyOnLoadList ??= new List<GameObject>();
        
        DontDestroyOnLoad(this);
        DontDestroyOnLoadList.Add(gameObject);
    }
}
