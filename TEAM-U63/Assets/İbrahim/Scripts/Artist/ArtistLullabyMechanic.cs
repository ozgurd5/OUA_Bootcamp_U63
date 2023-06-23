using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ArtistLullabyMechanic : MonoBehaviour
{
    public TextMeshProUGUI keyToPress;
    private bool isKeyPressed;
    private float currentTimer;
    public float timeToPressKey = 2f;
    private List<int> numberRange = new List<int>();

    public GameObject artistLullabyCanvas;

    private void Start()
    {
        GenerateNumberRange();
        ShuffleNumberRange();
        DisplayNextNumber();
        currentTimer = timeToPressKey;
    }

    private void Update()
    {
        //if (Input.GetKeyDown("e"))
        //{
        //    ActivateCanvas();
        //}
        
        
        if (isKeyPressed)
        {
            return;
        }

        if (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;
        }

        // Check if the player has pressed the correct number
        for (int i = 1; i <= 3; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                if (i == numberRange[0])
                {
                    Debug.Log("Correct!");
                }
                else
                {
                    Debug.Log("Incorrect!");
                    DeactivateCanvas();
                }

                isKeyPressed = true;
                ResetKeyPress();
                break;
            }
        }
        
        if (Input.GetKeyDown("e"))
        {
            if (artistLullabyCanvas.activeSelf)
            {
                DeactivateCanvas();
            }
            else
            {
                ActivateCanvas();
            }
        }

        if (currentTimer <= 0f)
        {
            DeactivateCanvas();
        }
    }

    private void ResetKeyPress()
    {
        isKeyPressed = false;
        currentTimer = timeToPressKey;
        ShuffleNumberRange();
        DisplayNextNumber();
    }

    private void GenerateNumberRange()
    {
        for (int i = 1; i <= 3; i++)
        {
            numberRange.Add(i);
        }
    }

    private void ShuffleNumberRange()
    {
        System.Random rng = new System.Random();
        int n = numberRange.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int temp = numberRange[k];
            numberRange[k] = numberRange[n];
            numberRange[n] = temp;
        }
    }

    private void DisplayNextNumber()
    {
        if (numberRange.Count > 0)
        {
            keyToPress.text = numberRange[0].ToString();
        }
    }

    private void ActivateCanvas()
    {
        
            artistLullabyCanvas.SetActive(true);
            isKeyPressed = false;
            currentTimer = timeToPressKey;

    }
    

    private void DeactivateCanvas()
    {
        artistLullabyCanvas.SetActive(false);
        isKeyPressed = false;

    }
}






