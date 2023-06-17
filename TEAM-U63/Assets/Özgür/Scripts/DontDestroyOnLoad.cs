using UnityEngine;

/// <summary>
/// <para>Prevents Unity to destroy the game object when the scene changes</para>
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
