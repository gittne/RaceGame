using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CheckpointHUD : MonoBehaviour
{
    [SerializeField] SCR_CheckpointManager checkpointManager;

    // Start is called before the first frame update
    void Start()
    {
        checkpointManager.OnDriverCorrectCheckpoint += OnDriverCorrectCheckpoint;
        checkpointManager.OnDriverWrongCheckpoint += OnDriverWrongCheckpoint;

        Hide();
    }

    void OnDriverCorrectCheckpoint(object sender, System.EventArgs e)
    {
        Hide();
    }

    void OnDriverWrongCheckpoint(object sender, System.EventArgs e)
    {
        Show();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
