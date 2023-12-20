using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Component_Rotation : MonoBehaviour
{
    [SerializeField] Image loading_image;
    float velocity = 75;

    private void FixedUpdate()
    {
        loading_image.transform.Rotate(-Vector3.forward * Time.fixedDeltaTime * velocity);
    }
}