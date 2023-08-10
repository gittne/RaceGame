using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CheckpointSingle : MonoBehaviour
{
    SCR_CheckpointManager checkpointManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kart") == true)
        {
            checkpointManager.DriverThroughCheckpoint(this);
        }
    }

    public void SetCheckpoints(SCR_CheckpointManager checkpointManager)
    {
        this.checkpointManager = checkpointManager;
    }
}
