using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Car_Controller : MonoBehaviourPunCallbacks
{
    #region VARS
    [Header("ACCELERATION SETTRINGS")]
    [SerializeField] float acceleration_multiplayer;
    [SerializeField] float acceleration_max;
    [SerializeField] float acceleration_break;

    [Space]
    [Header("TURN SETTRINGS")]
    [SerializeField] float turn_sensativity;
    [SerializeField] float max_steer_angle;

    [Space]
    [Header("WHEELS | EFFECTS")]
    [SerializeField] List<Wheel> wheels;
    [SerializeField] AudioSource source_drift;

    [Space]
    [Header("HEADLIGHTS")]
    [SerializeField] ParticleSystem headlight_L;
    [SerializeField] ParticleSystem headlight_R;

    [Space]
    [Header("DESTROY ON NETWORK LOAD")]
    [SerializeField] Camera _camera;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject event_system;
    [SerializeField] Car_Controller car_controller;
    [SerializeField] Drift_Score drift_score;

    float move_input;
    float steer_input;
    Rigidbody car_rb;
    PhotonView pv;
    private Vector3 center_of_mass;

    public enum Control_Mode
    {
        Keyboard,
        Touch_Buttons
    };

    public Control_Mode control_type;

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheel_model;
        public WheelCollider wheel_collider;
        public GameObject wheel_effect;
        public Axel axel;
    }

    #endregion

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)        
            car_controller = this;        
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            car_rb = GetComponent<Rigidbody>();
            car_rb.centerOfMass = center_of_mass;
        }
        else
        {
            Destroy(_camera.gameObject);
            Destroy(car_controller);
            Destroy(drift_score);
            Destroy(event_system);
            Destroy(canvas);
        }
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        Get_Inputs();
        Animate_Wheels();
    }

    private void LateUpdate()
    {
        if (!pv.IsMine) return;

        Move();
        Steer();
        Brake();
        Get_Wheels_Friction();
    }

    #region INPUT
    void Get_Inputs()
    {
        if (control_type == Control_Mode.Keyboard)
        {
            move_input = Input.GetAxis("Vertical");
            steer_input = Input.GetAxis("Horizontal");
        }
    }

    public void Input_Move(float _tmp_input)
    {
        move_input = _tmp_input;

        Check_Move_Direction();
    }

    public void Input_Steer(float _tmp_input)
    {
        steer_input = _tmp_input;

    }

    public void Disable_Movement()
    {
        move_input = 0;
        steer_input = 0;
    }
    #endregion

    #region CONTROLLER
    void Move()
    {
        foreach (var wheel in wheels)
            wheel.wheel_collider.motorTorque = move_input * acceleration_multiplayer * acceleration_max * Time.deltaTime;
    }

    void Steer()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steer_angle = steer_input * turn_sensativity * max_steer_angle;
                wheel.wheel_collider.steerAngle = Mathf.Lerp(wheel.wheel_collider.steerAngle, _steer_angle, 0.6f);
            }
        }
    }

    public void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
                wheel.wheel_collider.brakeTorque = acceleration_break * Time.deltaTime;

            Wheel_Effects_Enable();
            pv.RPC(nameof(RPC_Wheel_Effects_Enable), RpcTarget.AllBuffered);
        }
        else
        {
            foreach (var wheel in wheels)
                wheel.wheel_collider.brakeTorque = 0;

            Wheel_Effects_Disable();
            pv.RPC(nameof(RPC_Wheel_Effects_Disable), RpcTarget.AllBuffered);
        }
    }
    #endregion

    #region WHEELS | DRIFT
    void Animate_Wheels()
    {
        foreach (Wheel wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;

            wheel.wheel_collider.GetWorldPose(out pos, out rot);
            wheel.wheel_model.transform.position = pos;
            wheel.wheel_model.transform.rotation = rot;
        }
    }

    void Wheel_Effects_Enable()
    {
        foreach (var wheel in wheels)
            wheel.wheel_effect.GetComponentInChildren<TrailRenderer>().emitting = true;

        source_drift.enabled = true;
    }

    void Check_Move_Direction()
    {
        if (move_input > 0)
            HeadLights_Disable();

        if (move_input < 0)
            HeadLights_Enable();
    }

    void Wheel_Effects_Disable()
    {
        foreach (var wheel in wheels)
            wheel.wheel_effect.GetComponentInChildren<TrailRenderer>().emitting = false;

        source_drift.enabled = false;
    }

    void Drift_Encounter_Start()
    {
        drift_score.Drift_Start();
    }

    void Drift_Encounter_End()
    {
        drift_score.Drift_End();
    }

    void Get_Wheels_Friction()
    {
        WheelHit hit;
        if (wheels[3].wheel_collider.GetGroundHit(out hit))
        {
            float friction = hit.sidewaysSlip; // Получить боковое трение

            if (friction > 0.175f || friction < -0.175f)
            {
                Wheel_Effects_Enable();
                Drift_Encounter_Start();

                pv.RPC(nameof(RPC_Wheel_Effects_Enable), RpcTarget.AllBuffered);
            }
            else
            {
                Wheel_Effects_Disable();
                Drift_Encounter_End();

                pv.RPC(nameof(RPC_Wheel_Effects_Disable), RpcTarget.AllBuffered);
            }
        }
    }
    #endregion

    #region HEADlIGHTS
    void HeadLights_Enable()
    {
        headlight_L.Play();
        headlight_R.Play();

        pv.RPC(nameof(RPC_HeadLights_Enable), RpcTarget.AllBuffered);
    }

    void HeadLights_Disable()
    {
        headlight_L.Stop();
        headlight_R.Stop();

        pv.RPC(nameof(RPC_HeadLights_Disable), RpcTarget.AllBuffered);
    }

    #endregion

    #region RPCS
    [PunRPC]
    void RPC_HeadLights_Enable()
    {
        headlight_L.Play();
        headlight_R.Play();
    }

    [PunRPC]
    void RPC_HeadLights_Disable()
    {
        headlight_L.Stop();
        headlight_R.Stop();
    }

    [PunRPC]
    void RPC_Wheel_Effects_Enable()
    {
        foreach (var wheel in wheels)
            wheel.wheel_effect.GetComponentInChildren<TrailRenderer>().emitting = true;

        source_drift.enabled = true;
    }

    [PunRPC]
    void RPC_Wheel_Effects_Disable()
    {
        foreach (var wheel in wheels)
            wheel.wheel_effect.GetComponentInChildren<TrailRenderer>().emitting = false;

        source_drift.enabled = false;
    }
    #endregion
}