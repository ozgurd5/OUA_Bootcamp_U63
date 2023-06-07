using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


namespace U63
{
    public class PlayerManager : MonoBehaviourPunCallbacks
{

    #region Private Methods

#if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
#endif

    #endregion

    #region MonoBehaviour Callbacks

         

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }
    
       #if UNITY_5_4_OR_NEWER
           public override void OnDisable()
           {
               // Always call the base to remove callbacks
               base.OnDisable ();
               UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
           }
       #endif

    #endregion
    
    
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
          


          #if UNITY_5_4_OR_NEWER
          // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
                  UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
          #endif
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}


