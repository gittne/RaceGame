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
    bool isKartGrounded;

    //Speed values
    [Header("Speed Values")]
    [SerializeField] float fwdSpeed;
    [SerializeField] float revSpeed;
    [SerializeField] float turnSpeed;

    //Drag values
    [Header("Drag Values")]
    [SerializeField] float airDrag;
    [SerializeField] float groundDrag;

    //Ground check values
    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;

    //Logicball values
    [Header("Logic Ball Rigidbody")]
    [SerializeField] Rigidbody logicBallRigidbody;

    private void Start()
    {
        //Detaches the logicball from the car
        logicBallRigidbody.transform.parent = null;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Accelerate");
        turnInput = Input.GetAxisRaw("Horizontal");

        //Adjusts the speed of the car
        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        //Sets karts position to logicball's position
        transform.position = logicBallRigidbody.transform.position;

        //Sets cars rotation
        float newRotation = turnInput * turnSpeed * Time.deltaTime;
        if (!isKartGrounded || logicBallRigidbody.velocity.magnitude > 1.3)
        {
            transform.Rotate(0, newRotation, 0, Space.World);
        }

        Debug.Log("The current speed of the kart is: " + logicBallRigidbody.velocity.magnitude);

        //Raycast ground check
        RaycastHit hit;
        isKartGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        //Rotates car to be parallel to ground
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        logicBallRigidbody.drag = isKartGrounded ? groundDrag : airDrag;
    }

    private void FixedUpdate()
    {
        if (isKartGrounded)
        {
            logicBallRigidbody.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            logicBallRigidbody.AddForce(transform.up * -30f);
        }
    }
}
