using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CheckpointManager : MonoBehaviour
{


    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        foreach (Transform checkpoints in checkpointsTransform)
        {
            Debug.Log(checkpoints);
        }
    }
}
