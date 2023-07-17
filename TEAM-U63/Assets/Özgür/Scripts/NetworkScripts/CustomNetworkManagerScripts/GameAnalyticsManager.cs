using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.SceneManagement;

public class GameAnalyticsManager : MonoBehaviour
{
    [Header("Scene Info")]
    [SerializeField] private string currentSceneName;
    [SerializeField] private string previousSceneName;
    public bool isFirstIslandCompleted;
    public bool isSecondIslandCompleted;

    [Header("Play Times")]
    public float firstIslandGeneralPlayTime;
    public float secondIslandGeneralPlayTime;

    private void Awake()
    {
        SceneManager.activeSceneChanged += (previousScene, currentScene) =>
        {
            //not working, idk why
            //previousSceneName = previousScene.name;
            
            previousSceneName = currentSceneName;
            currentSceneName = currentScene.name;
            
            if (previousSceneName == "Island 1") SendPlayTime("firstIslandGeneralPlayTime", firstIslandGeneralPlayTime);
            if (previousSceneName == "Island 2") SendPlayTime("secondIslandGeneralPlayTime", secondIslandGeneralPlayTime);
            
            if (currentSceneName == "Main Island")
            {
                GameObject.Find("AnalyticsBoard").GetComponentInChildren<TextMeshPro>().text = 
                $"First Island Play Time: {(int)firstIslandGeneralPlayTime} seconds\nSecond Island Play Time: " +
                $"{(int)secondIslandGeneralPlayTime} seconds";
                
                if (previousSceneName == "Island 1") isFirstIslandCompleted = true;
                else if (previousSceneName == "Island 2") isSecondIslandCompleted = true;
            }
        };
    }

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            
            //IDK why we need this list
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        
        catch (ConsentCheckException e)
        {
            Debug.Log(e);
        }
    }

    private void Update()
    {
        if (currentSceneName == "Island 1") IncreasePlayTime(ref firstIslandGeneralPlayTime);
        else if (currentSceneName == "Island 2") IncreasePlayTime(ref secondIslandGeneralPlayTime);
    }
    
    
    private void IncreasePlayTime(ref float playTime)
    {
        playTime += Time.deltaTime;
    }
    
    private void SendPlayTime(string eventName, float playTime)
    {
        Dictionary<string, object> playtimeParameters = new Dictionary<string, object>()
        {
            { "playTime", playTime }
        };
        
        AnalyticsService.Instance.CustomData(eventName, playtimeParameters);
        AnalyticsService.Instance.Flush();
    }
}
