using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CheckpointManager : MonoBehaviour
{
    public event EventHandler OnDriverCorrectCheckpoint;
    public event EventHandler OnDriverWrongCheckpoint;

    GameObject checkpointManagerObject;
    List<SCR_CheckpointSingle> checkpointSingleList;
    int nextCheckpointIndex;

    private void Awake()
    {
        checkpointManagerObject = gameObject;

        checkpointSingleList = new List<SCR_CheckpointSingle>();

        foreach (Transform checkpoints in checkpointManagerObject.transform)
        {
            GameObject checkpoint = checkpoints.gameObject;

            SCR_CheckpointSingle checkpointSingle = checkpoint.GetComponent<SCR_CheckpointSingle>();

            checkpointSingle.SetCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointIndex = 0;
    }

    public void DriverThroughCheckpoint(SCR_CheckpointSingle checkpointSingle)
    {
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointIndex)
        {
            Debug.Log("B�ttre, du k�rde igenom r�tt checkpoint");
            nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpointSingleList.Count;
            OnDriverCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("V�rre, du k�rde igenom fel checkpoint. Du har ju tappat det");
            OnDriverWrongCheckpoint?.Invoke(this, EventArgs.Empty);
        }
    }
}