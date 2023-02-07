using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool forward;
    public bool steering;
}

public class KartController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    [SerializeField] float topSpeed;
    [SerializeField] float boostMultiplier;
    [SerializeField] Camera cam;
    Rigidbody rigBod;

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    void Start()
    {
        rigBod = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        float motorForward = maxMotorTorque * Input.GetAxis("Accelerate");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        cam.fieldOfView = 60;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.forward)
            {
                axleInfo.leftWheel.motorTorque = motorForward;
                axleInfo.rightWheel.motorTorque = motorForward;
            }
            if (Input.GetButton("Boost") && Input.GetAxis("Vertical") > 0)
            {
                axleInfo.leftWheel.motorTorque = motorForward * boostMultiplier;
                axleInfo.rightWheel.motorTorque = motorForward * boostMultiplier;
                cam.fieldOfView = 90;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        Debug.Log(motorForward);

        if (Input.GetButton("Drift"))
        {

        }

        if (Input.GetButtonDown("Reset"))
        {
            GameObject.FindGameObjectWithTag("TestCar").transform.position = new Vector3(142, 0.94f, 228);
            GameObject.FindGameObjectWithTag("TestCar").transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
