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

    [Header("First Island")]
    public float firstIslandGeneralPlayTime;
    //public float firstIslandFirstLevelPlayTime;
    //public float firstIslandSecondLevelPlayTime;
    
    [Header("Second Island")]
    public float secondIslandGeneralPlayTime;
    //public float secondIslandFirstLevelPlayTime;
    //public float secondIslandSecondLevelPlayTime;
    //public float secondIslandThirdLevelPlayTime;

    private void Awake()
    {
        SceneManager.activeSceneChanged += (previousScene, currentScene) =>
        {
            previousSceneName = previousScene.name;
            currentSceneName = currentScene.name;

            if (currentSceneName == "TEST")
            {
                GameObject.Find("AnalyticsBoard").GetComponentInChildren<TextMeshPro>().text = 
                $"First Island Time: {firstIslandGeneralPlayTime}\nSecond Island Time: {secondIslandGeneralPlayTime}";
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
        
        if (previousSceneName == "Island 1") SendPlayTime("firstIslandGeneralPlayTime", firstIslandGeneralPlayTime);
        if (previousSceneName == "Island 2") SendPlayTime("secondIslandGeneralPlayTime", secondIslandGeneralPlayTime);
    }
    
    
    private void IncreasePlayTime(ref float playTime)
    {
        playTime += Time.deltaTime;
    }

    /// <summary>
    /// <para>Send the play time of the level</para>
    /// <para>Must work at the end of the level</para>
    /// </summary>
    /// <param name="eventName">Island and level number</param>
    /// <param name="playTime">Play time</param>
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
