using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanHack : MonoBehaviour
{
    public GameObject artistLullabyCanvas;
    public BoxCollider robotCollider;
    
    [SerializeField] List<Image> images;
    [SerializeField] List<Sprite> arrowKeySprites;
    
    private bool canHack;

    public float timeToHack = 10f;
    private float currentTimer;
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
                
                canHack = true;

                if (currentTimer > 0f)
                {
                    currentTimer -= Time.deltaTime;
                    return;
                }
                
                
            }
            if (!canHack)
            {
                DisableImages();
                return;
            }

            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in arrowKeys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        if (key == sequence[currentIndex])
                        {
                            // Correct key pressed, move to the next arrow key
                            currentIndex++;
                            currentTimer = timeToHack;
                            break;
                        }
                        else
                        {
                            // Wrong key pressed, trigger failure
                            TriggerFailure();
                            StartCoroutine(DelayBeforeReset());
                            return;
                        }
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

    private void TriggerFailure()
    {
        
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
        GenerateRandomSequence();
    }
    
}
