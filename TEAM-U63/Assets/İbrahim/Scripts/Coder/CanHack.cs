using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanHack : MonoBehaviour
{
    public GameObject hackCanvas;
    public GameObject artistLullabyCanvas;
    public BoxCollider robotCollider;

    [SerializeField] List<Image> images;
    [SerializeField] List<Sprite> arrowKeySprites;

    private bool canHack;
    private bool isHacked;

    public float timeToHack = 10f;
    public float currentTimer;
    public float delayBetweenAttempts = 3f;
    private int currentIndex;

    public KeyCode[] arrowKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    private List<KeyCode> sequence;
    private System.Random rng;


    private void Start()
    {
        //robotCollider = GetComponent<BoxCollider>();
        rng = new System.Random();
        GenerateRandomSequence();

    }

    private void Update()
    {


        if (artistLullabyCanvas.activeSelf)
        {
            ;
            if (Input.GetKeyDown("c"))
            {
                
                hackCanvas.SetActive(true);
                canHack = true;
                currentTimer = timeToHack;


            }
            
            if (currentTimer > 0f)
            {
                currentTimer -= Time.deltaTime;
                
                bool corectKeyPressed = false;

                if (currentTimer <= 0)
                {
                    TriggerFailure();
                }
                
                if (Input.anyKeyDown && Input.GetKeyDown("c") == false)
                {
                    foreach (KeyCode key in arrowKeys)
                    {
                        if (Input.GetKeyDown(key))
                        {
                            if (key == sequence[currentIndex])
                            {
                                // Correct key pressed, move to the next arrow key
                                currentIndex++;
                                corectKeyPressed = true;
                            

                                break;
                            }


                        
                        }
                    }
                    if (currentIndex == sequence.Count)
                    {
                        TriggerSuccess();
                        Debug.Log("success trigger");
                    }

                    if (!corectKeyPressed)
                    {
                    
                        // Wrong key pressed, trigger failure
                        TriggerFailure();
                        Debug.Log("fail trigger");

                        
                    
                    }
                }
                    
            }

            

            
        }
    }

    private void GenerateRandomSequence()
    {
        sequence = new List<KeyCode>(arrowKeys);
        ShuffleSequence();
        //currentIndex = 0;
        canHack = true;
        currentTimer = timeToHack;

        // Assign arrow key sprites to images
        for (int i = 0; i < images.Count; i++)
        {
            if (i < sequence.Count)
            {
                int arrowIndex = Array.IndexOf(arrowKeys, sequence[i]);
                if (arrowIndex != -1)
                {
                    images[i].sprite = arrowKeySprites[arrowIndex];
                    images[i].gameObject.SetActive(true);

                    Debug.Log("Press " + arrowKeys[arrowIndex] + " key.");
                }
            }
            else
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }

    private void ShuffleSequence()
    {
        int n = sequence.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            KeyCode temp = sequence[k];
            sequence[k] = sequence[n];
            sequence[n] = temp;
        }
    }


    private void TriggerSuccess()
    {
        isHacked = true;
        hackCanvas.SetActive(false);
        currentIndex = 0;
        GenerateRandomSequence();
    }

    private void TriggerFailure()
    {
        
        hackCanvas.SetActive(false);
        currentIndex = 0;
        GenerateRandomSequence();
        StartCoroutine(DelayBeforeReset());
    }

    private void DisableImages()
    {
        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }
    }

    private IEnumerator DelayBeforeReset()
    {
        canHack = false;
        yield return new WaitForSeconds(delayBetweenAttempts);
        
        
        
    }
}
    

