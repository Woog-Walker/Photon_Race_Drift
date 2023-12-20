using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Settings : MonoBehaviour
{
    [SerializeField] int FPS_target;
    private float deltaTime = 0.0f;

    [SerializeField] Canvas_InGame_Manager car_canvas_manager;

    private void Awake()
    {
        Application.targetFrameRate = FPS_target;
    }

    private void Update()
    {
        Check_FPS();
    }

    void Check_FPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        float rounded_value = Mathf.Round(fps);
        car_canvas_manager.Update_UI_FPS(rounded_value);
    }
}