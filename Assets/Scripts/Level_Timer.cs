using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Timer : MonoBehaviour
{
    [SerializeField] float time_to_race;
    [SerializeField] float current_time;
    [Space]
    [SerializeField] Canvas_InGame_Manager canvas_manager;
    [SerializeField] Car_Controller car_controller;
    [SerializeField] Drift_Score drift_score;

    private void Start()
    {
        current_time = time_to_race;
        StartCoroutine(Deduct_Time());
    }

    public void Start_Timer()
    {
        StartCoroutine(Deduct_Time());
    }

    IEnumerator Deduct_Time()
    {
        yield return new WaitForSeconds(1);

        current_time -= 1;

        if (current_time >= 0)
        {
            Count_Time();
            StartCoroutine(Deduct_Time());
        }

        if (current_time == 0)
            End_Game();
    }

    void Count_Time()
    {
        float minutes = Mathf.FloorToInt(current_time / 60);
        float seconds = Mathf.FloorToInt(current_time % 60);

        canvas_manager.Update_UI_Time(minutes, seconds);
    }

    void End_Game()
    {
        canvas_manager.Show_EndGame_UI();
        car_controller.Disable_Movement();

        drift_score.Fill_EndGame_Info();
    }
}