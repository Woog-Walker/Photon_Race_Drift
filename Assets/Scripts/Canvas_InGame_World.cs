using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Canvas_InGame_World : MonoBehaviour
{
    [SerializeField] GameObject panel_score;
    [SerializeField] TMP_Text text_score;

    public void Update_Score_Text(float _tmp_score)
    {
        text_score.text = _tmp_score.ToString();
    }

    public void Score_Tab_Enable() { panel_score.SetActive(true); }
    public void Score_Tab_Disable() { panel_score.SetActive(false); }
}