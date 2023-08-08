using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WheelController : MonoBehaviour
{
    [SerializeField] GameObject[] wheelsToRotate;
    [SerializeField] float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float gasInput = Input.GetAxisRaw("Accelerate");

        foreach (var wheel in wheelsToRotate)
        {
            wheel.transform.Rotate(Time.deltaTime * gasInput * rotationSpeed, 0, 0, Space.Self);
        }
    }
}
