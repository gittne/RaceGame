using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WheelController : MonoBehaviour
{
    SCR_KartController kartController;

    //Wheel related values
    [Header("Wheel Related Fields")]
    [SerializeField] GameObject[] wheelsToRotate;
    [SerializeField] float rotationSpeed;
    [SerializeField] ParticleSystem[] smokeParticles;
    [SerializeField] TrailRenderer[] skidmarks;
    bool isDrifting;
    bool isBoosting;

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
        kartController = GetComponent<SCR_KartController>();
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
        if (Input.GetButton("Drift") && Input.GetAxisRaw("Accelerate") != 0 && !isBoosting && kartController.isGrounded)
        {
            foreach (ParticleSystem driftSmoke in smokeParticles)
            {
                driftSmoke.Emit(1);
                isDrifting = true;
            }
            if (kartController.isGrounded)
            {
                foreach (TrailRenderer skidmarks in skidmarks)
                {
                    skidmarks.emitting = true;
                }
            }
        }
        else
        {
            isDrifting = false;
            foreach (TrailRenderer skidmarks in skidmarks)
            {
                skidmarks.emitting = false;
            }
        }

        if (Input.GetButton("Boost") && Input.GetAxisRaw("Accelerate") != 0 && !isDrifting && kartController.boostBuildup < kartController.boostThreshold)
        {
            foreach (ParticleSystem boostFire in boostParticles)
            {
                boostFire.Emit(1);
                isBoosting = true;
            }
            if (kartController.isGrounded)
            {
                foreach (TrailRenderer skidmarks in skidmarks)
                {
                    skidmarks.emitting = true;
                }
            }
        }
        else
        {
            isBoosting = false;
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
