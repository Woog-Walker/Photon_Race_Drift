using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Info_Holder : MonoBehaviour
{
    [SerializeField] TMP_Text text_nick;
    [SerializeField] TMP_Text text_score;

    public void Set_Info(string _nick, float _score)
    {
        text_nick.text = _nick;
        text_score.text = _score.ToString();
    }

}