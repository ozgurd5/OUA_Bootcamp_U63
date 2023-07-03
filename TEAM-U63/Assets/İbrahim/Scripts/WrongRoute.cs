using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongRoute : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = respawnPoint.transform.position;
    }
}