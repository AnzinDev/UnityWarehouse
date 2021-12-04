using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualControl : MonoBehaviour
{
    public List<WheelCollider> DrivingWheels;
    public List<WheelCollider> FrontSteeringWheels;
    public List<WheelCollider> RearSteeringWheels;

    private Rigidbody rigidbody;

    private Vector3 StartPosition;
    private Quaternion StartRotation;

    public int currentSpeed;

    private float maxSteerAngle = 40;
    private float maxDriveTorque;

    private float[] speeds = new float[]
    {
        0, 100, 200, 400, 600,
    };

    private float maxBrakeTorque = 6000;

    void Start()
    {
        maxDriveTorque = speeds[0];
        currentSpeed = 0;
        rigidbody = GetComponent<Rigidbody>();
        StartPosition = transform.position;
        StartRotation = transform.rotation;
    }

    private void Update()
    {
        SpeedControl();
        Push();
    }

    void FixedUpdate()
    {
        DrivingControls();
        RecoverPosition();
    }

    void RecoverPosition()
    {
        if (transform.position.y < -20)
        {
            transform.position = StartPosition + new Vector3(0, 0.0001f, 0);
            transform.rotation = StartRotation;
            SpeedControl(0);
            AllBrakesOn();
            rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }

    void AllBrakesOn()
    {
        for (int i = 0; i < DrivingWheels.Count; i++)
        {
            DrivingWheels[i].motorTorque = 0;
        }

        for (int i = 0; i < DrivingWheels.Count; i++)
        {
            DrivingWheels[i].brakeTorque = maxBrakeTorque;
        }
    }

    void AllBrakesOff()
    {
        for (int i = 0; i < DrivingWheels.Count; i++)
        {
            DrivingWheels[i].brakeTorque = 0;
        }
    }

    void AllWheelDriveForward()
    {
        for (int i = 0; i < DrivingWheels.Count; i++)
        {
            DrivingWheels[i].motorTorque = maxDriveTorque;
        }
    }

    void AllWheelDriveBackward()
    {
        for (int i = 0; i < DrivingWheels.Count; i++)
        {
            DrivingWheels[i].motorTorque = -maxDriveTorque;
        }
    }

    void AllWheelStop()
    {
        for (int i = 0; i < DrivingWheels.Count; i++)
        {
            DrivingWheels[i].motorTorque = 0;
        }
    }

    void SteerLeft()
    {
        for (int i = 0; i < FrontSteeringWheels.Count; i++)
        {
            FrontSteeringWheels[i].steerAngle = -maxSteerAngle;
            RearSteeringWheels[i].steerAngle = maxSteerAngle;
        }
    }

    void SteerRight()
    {
        for (int i = 0; i < FrontSteeringWheels.Count; i++)
        {
            FrontSteeringWheels[i].steerAngle = maxSteerAngle;
            RearSteeringWheels[i].steerAngle = -maxSteerAngle;
        }
    }

    void SteerStraight()
    {
        for (int i = 0; i < FrontSteeringWheels.Count; i++)
        {
            FrontSteeringWheels[i].steerAngle = 0;
            RearSteeringWheels[i].steerAngle = 0;
        }
    }

    void Push()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rigidbody.AddForce(transform.forward * 1000, ForceMode.Impulse);
        }
    }

    void DrivingControls()
    {
        SteerStraight();
        if (Input.GetKey(KeyCode.S))
        {
            AllBrakesOff();
            AllWheelDriveForward();
        }
        if (Input.GetKey(KeyCode.W))
        {
            AllBrakesOff();
            AllWheelDriveBackward();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            AllWheelStop();
            AllBrakesOn();
        }
        if (Input.GetKey(KeyCode.A))
        {
            SteerLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            SteerRight();
        }
    }

    void SpeedControl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            maxDriveTorque = speeds[1];
            currentSpeed = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            maxDriveTorque = speeds[2];
            currentSpeed = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            maxDriveTorque = speeds[3];
            currentSpeed = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            maxDriveTorque = speeds[4];
            currentSpeed = 4;
        }
    }

    void SpeedControl(int i)
    {
        maxDriveTorque = speeds[i];
        currentSpeed = i;
    }
}


