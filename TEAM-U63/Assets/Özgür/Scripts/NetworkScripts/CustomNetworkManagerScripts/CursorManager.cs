using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: USE EVENTS
public class CursorManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject pauseMenuCanvas;
    
    private string currentSceneName;
    
    private void Awake()
    {
        SceneManager.activeSceneChanged += (a, currentScene) =>
        {
            currentSceneName = currentScene.name;
        };
    }

    private void Update()
    {
        if (currentSceneName == "MAIN_MENU")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {
            if (pauseMenuCanvas.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
