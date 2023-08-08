using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WheelController : MonoBehaviour
{
    //Wheel related values
    [Header("Wheel Related Fields")]
    [SerializeField] GameObject[] wheelsToRotate;
    [SerializeField] float rotationSpeed;

    //Steering axel related values
    [Header("Steering Axel Fields")]
    [SerializeField] GameObject steeringAxel;
    [SerializeField] Animator axelAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateWheels();
        RotateAxel();
    }

    void RotateWheels()
    {
        float gasInput = Input.GetAxisRaw("Accelerate");

        foreach (var wheel in wheelsToRotate)
        {
            wheel.transform.Rotate(Time.deltaTime * gasInput * rotationSpeed, 0, 0, Space.Self);
        }
    }

    void RotateAxel()
    {
        float steeringInput = Input.GetAxisRaw("Horizontal");

        if (steeringInput > 0)
        {
            axelAnimator.SetBool("goingLeft", false);
            axelAnimator.SetBool("goingRight", true);
        }
        else if(steeringInput < 0)
        {
            axelAnimator.SetBool("goingRight", false);
            axelAnimator.SetBool("goingLeft", true);
        }
        else
        {
            axelAnimator.SetBool("goingLeft", false);
            axelAnimator.SetBool("goingRight", false);
        }
    }
}
