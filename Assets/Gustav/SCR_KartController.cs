using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SCR_KartController : MonoBehaviour
{
    //Script by Gustav Eriksson
    //This script is responsible for making the kart gameobject 
    //to follow a sphere gameobject that manages all the logic
    //Base code provided by SpawnCampGames: https://www.youtube.com/watch?v=TBIYSksI10k

    //Logic values
    float gasInput;
    float reverseInput;
    float turnInput;
    float turnLogicValue;
    float standardSpeed;
    float standardGroundDrag;
    float boostValue;
    float airtime;
    float pushResetTimer;
    float pushDamageTimer;
    bool isKartGrounded;
    bool isDrifting;
    bool isBoosting;

    //Speed values
    [Header("Movement Values")]
    [SerializeField] float fwdSpeed;
    [SerializeField] float revSpeed;
    [SerializeField] float driftSpeed;
    [SerializeField] float boostSpeed;

    //Boost Values
    [Header("Boost Values")]
    [SerializeField] float standardBoostBuildup;
    float standardBoostThreshold = 100;
    [SerializeField] float standardBoostDriftRefill;
    [SerializeField] float standardBoostAirtimeRefill;
    [SerializeField] float standardBoostAirtimeBuffert;
    [SerializeField] float standardBoostPushCost;

    //Turn values
    [Header("Turn Values")]
    [SerializeField] float standardTurnValue;
    [SerializeField] float driftTurnValue;
    [SerializeField] float boostTurnValue;
    [SerializeField] float speedTurnThreshold;

    //Side push values
    [Header("Side Pushing Values")]
    [SerializeField] float pushForce;
    [SerializeField] float pushResetThreshold;

    //Align to ground values
    [Header("Ground Alignment Values")]
    [SerializeField] float alignmenSpeed;
    [SerializeField] float axisResetMultiplier;

    //Drag values
    [Header("Drag Values")]
    [SerializeField] float airDrag;
    [SerializeField] float groundDrag;
    [SerializeField] float driftDrag;

    //Ground check values
    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;

    //Rigidbodies
    [Header("Rigidbodies")]
    [SerializeField] Rigidbody logicBallRigidbody;
    [SerializeField] Rigidbody kartRigidbody;

    private void Start()
    {
        //Detaches the logicball and rigidbody from the kart
        logicBallRigidbody.transform.parent = null;
        kartRigidbody.transform.parent = null;

        //Set standard values to default values
        turnLogicValue = standardTurnValue;
        standardSpeed = fwdSpeed;
        standardGroundDrag = groundDrag;
        boostValue = 0;
        airtime = 0;
    }

    private void Update()
    {
        //Movement inputs
        gasInput = Input.GetAxisRaw("Accelerate");
        reverseInput = Input.GetAxisRaw("Reverse");
        turnInput = Input.GetAxisRaw("Horizontal");

        GasReverseInput();

        //Sets karts position to logicball's position
        transform.position = logicBallRigidbody.transform.position;

        TurnInput();

        RotationCheck();

        SidePush();

        //Checks if kart is in the air and changes drag accordingly
        logicBallRigidbody.drag = isKartGrounded ? groundDrag : airDrag;
    }

    private void FixedUpdate()
    {
        AddForceToKart(gasInput, reverseInput);

        //Rotates the rigidbody of the kart
        kartRigidbody.MoveRotation(transform.rotation);
    }

    void AddForceToKart(float gas, float reverse)
    {
        //Forces added to the logicball in order to move it when on the ground
        if (isKartGrounded)
        {
            //Set each axis of the triggers to 0 if not pressed
            if (gas <= 0)
            {
                gas = 0;
            }
            if (reverse <= 0)
            {
                reverse = 0;
            }
            //Adds forces to the logic ball rigidbody
            logicBallRigidbody.AddForce(transform.forward * gas, ForceMode.Acceleration);
            logicBallRigidbody.AddForce(transform.forward * -reverse, ForceMode.Acceleration);
        }
        //Otherwise it sends the ball down at a constant speed
        else
        {
            logicBallRigidbody.AddForce(transform.up * -40f);
        }
    }

    void GasReverseInput()
    {
        //Checks if the player is boosting, if so then set speed to boost speed
        if (Input.GetButton("Boost") && gasInput > 0 && !isDrifting && boostValue < standardBoostThreshold)
        {
            gasInput *= boostSpeed;
            turnLogicValue = boostTurnValue;
            boostValue += standardBoostBuildup * Time.deltaTime;
            isBoosting = true;
        }
        //If not, set the speed to default speed
        else
        {
            gasInput *= fwdSpeed;
            reverseInput *= revSpeed;
            isBoosting = false;
        }
        //Resets boost if the debug button is pressed
        if (Input.GetButton("DebugTimer"))
        {
            boostValue = 0;
        }
    }

    void RotationCheck()
    {
        //Raycast ground check
        RaycastHit hit;
        isKartGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer)
            || Physics.Raycast(transform.position, -transform.forward, out hit, 1f, groundLayer)
            || Physics.Raycast(transform.position, transform.forward, out hit, 1f, groundLayer)
            || Physics.Raycast(transform.position, -transform.right, out hit, 1f, groundLayer)
            || Physics.Raycast(transform.position, transform.right, out hit, 1f, groundLayer);

        //Rotates kart to be parallel to ground
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, alignmenSpeed * Time.deltaTime);

        //Rotates kart to re-center on the X and Z axis when in the air
        Quaternion currentAngles = transform.rotation;
        float yRotation = currentAngles.eulerAngles.y;
        Quaternion currentRotation = Quaternion.Euler(0, yRotation, 0);
        if (!isKartGrounded)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, axisResetMultiplier * Time.deltaTime);

            //Checks if the kart has spent a certain amount of time in the air
            //If so then start filling up the boost
            airtime += Time.deltaTime;
            if (airtime > standardBoostAirtimeBuffert)
            {
                boostValue -= standardBoostAirtimeRefill * Time.deltaTime;
            }
        }
        //Resets the airtime when on the ground
        else
        {
            airtime = 0;
        }
    }

    void SidePush()
    {
        //Timer which keeps track if the kart can push to either side
        pushResetTimer += Time.deltaTime;
        pushDamageTimer += Time.deltaTime;

        //Pushes the kart to the left
        if (Input.GetButtonDown("SidePushLeft") && pushResetTimer > pushResetThreshold && boostValue < standardBoostThreshold)
        {
            logicBallRigidbody.AddForce(-transform.right * pushForce);
            boostValue += standardBoostPushCost;
            pushResetTimer = 0;
            pushDamageTimer = 0;
        }

        //Pushes the kart to the right
        if (Input.GetButtonDown("SidePushRight") && pushResetTimer > pushResetThreshold && boostValue < standardBoostThreshold)
        {
            logicBallRigidbody.AddForce(transform.right * pushForce);
            boostValue += standardBoostPushCost;
            pushResetTimer = 0;
            pushDamageTimer = 0;
        }
    }

    void TurnInput()
    {
        //Sets cars rotation
        float newRotation = turnInput * turnLogicValue * Time.deltaTime;
        if (!isKartGrounded || logicBallRigidbody.velocity.magnitude > speedTurnThreshold)
        {
            //If the player's drifting the turning value increases and speed decreases
            if (Input.GetButton("Drift") && gasInput > 0 && turnInput != 0 && !isBoosting)
            {
                turnLogicValue = driftTurnValue;
                fwdSpeed = driftSpeed;
                groundDrag = driftDrag;
                boostValue -= standardBoostDriftRefill * Mathf.Abs(turnInput) * Time.deltaTime;
                if (boostValue < 0)
                {
                    boostValue = 0;
                }
                isDrifting = true;
            }
            //Otherwise keep the logic values at standard levels
            else
            {
                turnLogicValue = standardTurnValue;
                fwdSpeed = standardSpeed;
                groundDrag = standardGroundDrag;
                isDrifting = false;
            }
            //Rotates the kart accordingly
            transform.Rotate(0, newRotation, 0, Space.World);
        }
    }

    public bool isGrounded
    {
        get { return isKartGrounded; }
        set { isKartGrounded = value; }
    }
    public bool isUsingBoost
    {
        get { return isBoosting; }
        set { isBoosting = value; }
    }

    public float boostBuildup
    {
        get { return boostValue; }
        set { boostValue = value; }
    }

    public float boostThreshold
    {
        get { return standardBoostThreshold; }
        set { standardBoostThreshold = value; }
    }

    public Rigidbody logicBall
    {
        get { return logicBallRigidbody; }
        set { logicBallRigidbody = value; }
    }

    public float speedTurn
    {
        get { return speedTurnThreshold; }
        set { speedTurnThreshold = value; }
    }
}
