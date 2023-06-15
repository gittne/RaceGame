using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKartControllerModel
{
    void Velocity(float speed);
    void Steer(float angle);
    void Drift();
}
