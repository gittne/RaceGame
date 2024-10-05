using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SidewaysPushDamage : MonoBehaviour
{
    [SerializeField] float pushForceMultiplier;
    [SerializeField] GameObject[] raycastOrigins;
    [SerializeField] Ray[] rays;
    [SerializeField] int numberOfRays;

    private void Start()
    {
        rays = new Ray[numberOfRays];

        for (int i = 0; i < numberOfRays; i++)
        {
            rays[i] = new Ray(transform.position, transform.forward);
        }
    }
}