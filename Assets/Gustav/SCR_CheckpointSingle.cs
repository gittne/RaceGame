using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CheckpointSingle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kart"))
        {
            Debug.Log("Checkpoint, yay!");
        }
    }
}
