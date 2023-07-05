using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MapEnabler : MonoBehaviour
{
    public GameObject map;
    [SerializeField] GameObject mapImage;
    private bool mapCollected;
    private bool isMapActive;

    private void Update()
    {
        if (Input.GetKeyDown("m") && mapCollected)
        {
            isMapActive = !isMapActive;
            mapImage.SetActive(isMapActive);
            Debug.Log("key pressed");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == map)
        {
            Destroy(map);
            mapCollected = true;
            isMapActive = true;
            mapImage.SetActive(true);
            Debug.Log("image enabled");
        }

    }

}
