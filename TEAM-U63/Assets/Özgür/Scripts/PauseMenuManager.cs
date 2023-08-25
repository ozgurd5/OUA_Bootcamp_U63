using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
     private GameObject settings;
     private Canvas pauseMenuCanvas;
     
     private PlayerData coderPlayerData;
     private PlayerData artistPlayerData;
     private PlayerInputManager coderPim;
     private PlayerInputManager artistPim;
     private PlayerStateData coderPsd;
     private PlayerStateData artistPsd;
     
     private PlayerInputManager pim;
     private PlayerStateData psd;
     private PlayerStateData.PlayerMainState previousState;

     private void Awake()
     {
          settings = transform.Find("SETTINGS").gameObject;
          pauseMenuCanvas = GetComponent<Canvas>();
          
          Cursor.visible = false;
          Cursor.lockState = CursorLockMode.Locked;

          GetPlayerComponents();
          coderPlayerData.OnLocalStatusChanged += UpdatePimAndPsd;
     }

     private void GetPlayerComponents()
     {
          coderPlayerData = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
          artistPlayerData = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();

          coderPim = coderPlayerData.GetComponent<PlayerInputManager>();
          coderPsd = coderPlayerData.GetComponent<PlayerStateData>();

          artistPim = artistPlayerData.GetComponent<PlayerInputManager>();
          artistPsd = artistPlayerData.GetComponent<PlayerStateData>();
     }

     private void UpdatePimAndPsd()
     {
          if (coderPlayerData.isLocal)
          {
               pim = coderPim;
               psd = coderPsd;
          }
          
          else
          {
               pim = artistPim;
               psd = artistPsd;
          }
     }

     private void Update()
     {
          if (pim.isPauseKeyDown)
          {
               if (!pauseMenuCanvas.enabled) OpenMainMenu();
               else CloseMainMenu();
          }
     }

     private void OpenMainMenu()
     {
          Cursor.visible = true;
          Cursor.lockState = CursorLockMode.None;

          pauseMenuCanvas.enabled = true;
          
          previousState = psd.currentMainState;
          psd.currentMainState = PlayerStateData.PlayerMainState.RobotControllingState; //Player freezes in this state
     }

     public void CloseMainMenu()
     {
          Cursor.visible = false;
          Cursor.lockState = CursorLockMode.Locked;
          
          pauseMenuCanvas.enabled = false;
          settings.SetActive(false);
          
          psd.currentMainState = previousState;
     }
     
     public void ExitGame()
     {
          Application.Quit();
     }
}
