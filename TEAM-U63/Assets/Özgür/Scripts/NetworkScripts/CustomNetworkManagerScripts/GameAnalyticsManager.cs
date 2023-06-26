using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class GameAnalyticsManager : MonoBehaviour
{
    public static GameAnalyticsManager Singleton;
    
    public float firstIslandFirstLevelPlayTime;
    public float firstIslandSecondLevelPlayTime;
    public float secondIslandFirstLevelPlayTime;
    public float secondIslandSecondLevelPlayTime;
    public float secondIslandThirdLevelPlayTime;
    public float thirdIslandFirstLevelPlayTime;
    public float thirdIslandSecondLevelPlayTime;
    public float thirdIslandThirdLevelPlayTime;
    
    public int hackNumber;
    public int paintNumber;

    [SerializeField] private float testPlayTime;
    private int testEventNumber;

    private void Awake()
    {
        Singleton = GetComponent<GameAnalyticsManager>();
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
    
    /// <summary>
    /// <para>Increases play time of the level, but doesn't send the data</para>
    /// <para>Must work in Update</para>
    /// </summary>
    /// <param name="playTime">Island and level number</param>
    public void IncreasePlayTime(ref float playTime)
    {
        playTime += Time.deltaTime;
    }

    /// <summary>
    /// <para>Send the play time of the level</para>
    /// <para>Must work at the end of the level</para>
    /// </summary>
    /// <param name="eventName">Island and level number</param>
    /// <param name="playTime">Play time</param>
    public void SendPlayTime(string eventName, float playTime)
    {
        Dictionary<string, object> playtimeParameters = new Dictionary<string, object>()
        {
            { "playTime", playTime }
        };
        
        AnalyticsService.Instance.CustomData(eventName, playtimeParameters);
        AnalyticsService.Instance.Flush();
    }

    /// <summary>
    /// <para>Increases and sends the hack number</para>
    /// <para>Must work when a successful hacking preformed</para>
    /// </summary>
    public void IncreaseAndSendHackNumber()
    {
        hackNumber++;
        
        Dictionary<string, object> hackNumberParameters = new Dictionary<string, object>()
        {
            { "hackNumber", hackNumber }
        };
        
        AnalyticsService.Instance.CustomData("hackNumberEvent", hackNumberParameters);
        AnalyticsService.Instance.Flush();
    }

    /// <summary>
    /// <para>Increases and sends the paint number</para>
    /// <para>Must work when a thing painted</para>
    /// </summary>
    public void IncreaseAndSendPaintNumber()
    {
        paintNumber++;
        
        Dictionary<string, object> paintNumberParameters = new Dictionary<string, object>()
        {
            { "paintNumber", paintNumber }
        };
        
        AnalyticsService.Instance.CustomData("paintNumberEvent", paintNumberParameters);
        AnalyticsService.Instance.Flush();
    }

    //TEST
    private void SendTestEvent()
    {
        testEventNumber++;
        
        Dictionary<string, object> testEventParameters = new Dictionary<string, object>()
        {
            { "testParameter", testEventNumber }
        };

        AnalyticsService.Instance.CustomData("testEvent", testEventParameters);
        AnalyticsService.Instance.Flush();
    }

    private void Update()
    {
        //USE FOR TESTS ONLY
        IncreasePlayTime(ref testPlayTime);

        if (Input.GetKeyDown(KeyCode.T))
        {
            SendPlayTime("testPlayTime", testPlayTime);
        }
    }
}
