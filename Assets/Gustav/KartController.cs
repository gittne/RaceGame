using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class KartController : MonoBehaviour
{
    [SerializeField] List<AxleInfo> axleInfos;
    [SerializeField] float maxMotorTorque;
    [SerializeField] float maxSteeringAngle;
    [SerializeField] GameObject Body;

    // finds the corresponding visual wheel
    // correctly applies the transform
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

    public void Update()
    {
        float motor = maxMotorTorque * Input.GetAxisRaw("Vertical");
        float steering = maxSteeringAngle * Input.GetAxisRaw("Horizontal");

        if (motor > 0)
        {
            
        }

        // Check if the motor input is zero
        // Check if the gas/brake input is released
        //if (Input.GetButtonUp("Vertical"))
        //{
        //    motor = 0f; // Set motor torque to zero
        //    foreach (AxleInfo axleInfo in axleInfos)
        //    {
        //        if (axleInfo.motor)
        //        {
        //            axleInfo.leftWheel.brakeTorque = maxMotorTorque; // Apply brake torque
        //            axleInfo.rightWheel.brakeTorque = maxMotorTorque; // Apply brake torque
        //        }
        //    }
        //}
        //else
        //{
        //    foreach (AxleInfo axleInfo in axleInfos)
        //    {
        //        if (axleInfo.motor)
        //        {
        //            axleInfo.leftWheel.brakeTorque = 0f; // Reset brake torque
        //            axleInfo.rightWheel.brakeTorque = 0f; // Reset brake torque
        //            axleInfo.leftWheel.motorTorque = motor;
        //            axleInfo.rightWheel.motorTorque = motor;
        //        }
        //    }
        //}

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            //if (axleInfo.motor)
            //{
            //    axleInfo.leftWheel.motorTorque = motor;
            //    axleInfo.rightWheel.motorTorque = motor;
            //}
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}