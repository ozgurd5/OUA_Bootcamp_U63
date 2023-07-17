using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
     [Header("Assign")]
     [SerializeField] private GameObject pauseMenuCanvas;
     [SerializeField] private GameObject settings;
     
     [Header("INFO")]
     [SerializeField] private PlayerData coder;
     [SerializeField] private PlayerData artist;
     [SerializeField] private PlayerInputManager pim;
     [SerializeField] private PlayerStateData psd;
     [SerializeField] private string currentSceneName;
     [SerializeField] private PlayerStateData.PlayerMainState previousState;

     private void Awake()
     {
          SceneManager.activeSceneChanged += (a, currentScene) =>
          {
               currentSceneName = currentScene.name;
          };
     }

     private void Update()
     {
          if (currentSceneName == "MAIN_MENU") return;
          
          if (currentSceneName == "MAIN_MENU") return;

          coder = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
          artist = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();
          
          if (coder.isLocal)
          {
               pim = coder.GetComponent<PlayerInputManager>();
               psd = coder.GetComponent<PlayerStateData>();
          }
          else if (artist.isLocal)
          {
               pim = artist.GetComponent<PlayerInputManager>();
               psd = artist.GetComponent<PlayerStateData>();
          }
          
          if (pim.isPauseKeyDown)
          {
               pauseMenuCanvas.SetActive(!pauseMenuCanvas.activeSelf);
               
               //TODO: BETTER SOLUTION
               if (pauseMenuCanvas.activeSelf)
               {
                    previousState = psd.currentMainState;
                    psd.currentMainState = PlayerStateData.PlayerMainState.RobotControllingState;
               }

               else
               {
                    settings.SetActive(false);
                    psd.currentMainState = previousState;
               }
          }
     }

     public void ExitGame()
     {
          Application.Quit();
     }
}
