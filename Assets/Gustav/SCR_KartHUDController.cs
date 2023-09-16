using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_KartHUDController : MonoBehaviour
{
    [SerializeField] Rigidbody kart;
    [SerializeField] TextMeshProUGUI speedOMeter;
    float timer;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider boostBar;
    SCR_KartController kartController;

    // Start is called before the first frame update
    void Start()
    {
        kartController = GetComponent<SCR_KartController>();
    }

    // Update is called once per frame
    void Update()
    {
        Speed();
        Timer();
        Boost();
    }

    void Speed()
    {
        float speed = kart.velocity.magnitude * 3.6f;
        float roundedSpeed = Mathf.Round(speed * 100) / 100;
        speedOMeter.text = roundedSpeed.ToString() + " km/h";
    }

    void Timer()
    {
        timer += Time.deltaTime;
        float rounderTimer = Mathf.Round(timer * 100) / 100;
        timerText.text = rounderTimer.ToString() + " S";
        if (Input.GetButtonDown("DebugTimer"))
        {
            timer = 0;
        }
    }

    void Boost()
    {
        boostBar.value = (kartController.boostThreshold - kartController.boostBuildup) / kartController.boostThreshold;
    }
}
