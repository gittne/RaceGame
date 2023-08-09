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
    float moveInput;
    float turnInput;
    float turnLogicValue;
    float standardSpeed;
    float standardGroundDrag;
    bool isKartGrounded;
    bool isDrifting;
    bool isBoosting;

    //Speed values
    [Header("Movement Values")]
    [SerializeField] float fwdSpeed;
    [SerializeField] float revSpeed;
    [SerializeField] float driftSpeed;
    [SerializeField] float boostSpeed;

    //Turn values
    [Header("Turn Values")]
    [SerializeField] float standardTurnValue;
    [SerializeField] float driftTurnValue;
    [SerializeField] float boostTurnValue;

    //Align to ground values
    [Header("Ground Alignment")]
    [SerializeField] float alignmenSpeed;
    [SerializeField] GameObject groundChecker;

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
        //Detaches the logicball and rigidbody from the car
        logicBallRigidbody.transform.parent = null;
        kartRigidbody.transform.parent = null;
        //Set standard values;
        turnLogicValue = standardTurnValue;
        standardSpeed = fwdSpeed;
        standardGroundDrag = groundDrag;
    }

    private void Update()
    {
        //Movement inputs
        moveInput = Input.GetAxisRaw("Accelerate");
        turnInput = Input.GetAxisRaw("Horizontal");

        //Checks if the player is boosting
        if (Input.GetButton("Boost") && !isDrifting)
        {
            moveInput *=  boostSpeed;
            turnLogicValue = boostTurnValue;
            isBoosting = true;
        }
        else
        {
            moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;
            isBoosting = false;
        }

        //Sets karts position to logicball's position
        transform.position = logicBallRigidbody.transform.position;

        //Sets cars rotation
        float newRotation = turnInput * turnLogicValue * Time.deltaTime;
        if (!isKartGrounded || logicBallRigidbody.velocity.magnitude > 1.3)
        {
            //If the player's drifting the turning value increases and speed decreases
            if (Input.GetButton("Drift") && turnInput != 0 && !isBoosting)
            {
                turnLogicValue = driftTurnValue;
                fwdSpeed = driftSpeed;
                groundDrag = driftDrag;
                isDrifting = true;
                Debug.Log("Driftar, IEA");
            }
            else
            {
                turnLogicValue = standardTurnValue;
                fwdSpeed = standardSpeed;
                groundDrag = standardGroundDrag;
                isDrifting = false;
            }
            transform.Rotate(0, newRotation, 0, Space.World);
        }

        //Debug.Log("The current speed of the kart is: " + logicBallRigidbody.velocity.magnitude);

        //Raycast ground check
        RaycastHit hit;
        isKartGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        //Rotates kart to be parallel to ground
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, alignmenSpeed * Time.deltaTime);

        //Checks if kart is in the air and changes drag accordingly
        logicBallRigidbody.drag = isKartGrounded ? groundDrag : airDrag;
    }

    private void FixedUpdate()
    {
        //Forces added to the logicball in order to move it 
        if (isKartGrounded)
        {
            logicBallRigidbody.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            logicBallRigidbody.AddForce(transform.up * -30f);
        }

        kartRigidbody.MoveRotation(transform.rotation);
    }
}
