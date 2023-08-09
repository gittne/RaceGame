using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WheelController : MonoBehaviour
{
    //Wheel related values
    [Header("Wheel Related Fields")]
    [SerializeField] GameObject[] wheelsToRotate;
    [SerializeField] float rotationSpeed;
    [SerializeField] ParticleSystem[] smokeParticles;

    //Steering axel related values
    [Header("Steering Axel Fields")]
    [SerializeField] GameObject steeringAxel;
    [SerializeField] Animator axelAnimator;

    //Boost Effects
    [Header("Boost Particle Effects")]
    [SerializeField] ParticleSystem[] boostParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateWheels();
        RotateAxel();
        ParticleEffects();
    }

    void ParticleEffects()
    {
        if (Input.GetButton("Drift") && Input.GetAxisRaw("Accelerate") != 0)
        {
            foreach (ParticleSystem driftSmoke in smokeParticles)
            {
                driftSmoke.Emit(1);
            }
        }

        if (Input.GetButton("Boost") && Input.GetAxisRaw("Accelerate") != 0)
        {
            foreach (ParticleSystem boostFire in boostParticles)
            {
                boostFire.Emit(1);
            }
        }
    }

    //Rotates the wheels around each axels
    void RotateWheels()
    {
        float gasInput = Input.GetAxisRaw("Accelerate");

        foreach (var wheel in wheelsToRotate)
        {
            wheel.transform.Rotate(Time.deltaTime * gasInput * rotationSpeed, 0, 0, Space.Self);
        }
    }

    //Rotates the front axel when steering
    void RotateAxel()
    {
        float steeringInput = Input.GetAxisRaw("Horizontal");
        float gasInput = Input.GetAxisRaw("Accelerate");

        if (steeringInput > 0 && gasInput > 0)
        {
            axelAnimator.SetBool("turnLeft", false);
            axelAnimator.SetBool("turnRight", true);
        }
        else if(steeringInput < 0 && gasInput > 0)
        {
            axelAnimator.SetBool("turnRight", false);
            axelAnimator.SetBool("turnLeft", true);
        }
        else if(steeringInput > 0 && gasInput < 0)
        {
            axelAnimator.SetBool("turnRight", false);
            axelAnimator.SetBool("turnLeft", true);
        }
        else if (steeringInput < 0 && gasInput < 0)
        {
            axelAnimator.SetBool("turnLeft", false);
            axelAnimator.SetBool("turnRight", true);
        }
        else
        {
            axelAnimator.SetBool("turnLeft", false);
            axelAnimator.SetBool("turnRight", false);
        }
    }
}
