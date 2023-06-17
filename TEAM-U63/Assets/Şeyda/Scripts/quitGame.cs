using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class quitGame : MonoBehaviour
{
    public void quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
