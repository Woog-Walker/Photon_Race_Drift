using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Audio_Engine : MonoBehaviour
{
    [SerializeField] float speed_min;
    [SerializeField] float speed_max;
    [SerializeField] float speed_current;
    [Space]
    [SerializeField] float pitch_min;
    [SerializeField] float pitch_max;
    [SerializeField] float pitch_from_car;
    [Space]
    [SerializeField] AudioSource audio_source;
    [SerializeField] AudioClip sound_engine;
    [SerializeField] Rigidbody rb;

    private void Update()
    {
        Engine_Sound();
    }

    void Engine_Sound()
    {
        speed_current = rb.velocity.magnitude;
        pitch_from_car = rb.velocity.magnitude / 50;

        audio_source.pitch = (pitch_from_car + 1) * 1.25f;
    }
}