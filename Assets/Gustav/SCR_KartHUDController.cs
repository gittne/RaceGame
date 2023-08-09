using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_KartHUDController : MonoBehaviour
{
    [SerializeField] Rigidbody kart;
    [SerializeField] TextMeshProUGUI speedOMeter;
    float timer;
    [SerializeField] TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = kart.velocity.magnitude * 3.6f;
        float roundedSpeed = Mathf.Round(speed * 100) / 100;
        speedOMeter.text = roundedSpeed.ToString() + " km/h";
        timer += Time.deltaTime;
        float rounderTimer = Mathf.Round(timer * 100) / 100;
        timerText.text = rounderTimer.ToString() + " S";
        if (Input.GetButtonDown("DebugTimer"))
        {
            timer = 0;
        }
    }
}
