using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Canvas_InGame_Manager : MonoBehaviour
{
    [SerializeField] TMP_Text text_FPS;
    [SerializeField] TMP_Text text_time;
    [SerializeField] TMP_Text text_score;
    [Space]
    [SerializeField] GameObject panel_over;
    [SerializeField] GameObject holder_refab;
    [SerializeField] Transform info_place_holder;
    [Space]
    [SerializeField] List<GameObject> objs_to_disable = new List<GameObject>();

    public void Update_UI_FPS(float _tmp)
    {
        text_FPS.text = _tmp.ToString();
    }

    public void Update_UI_Score(float _tmp)
    {
        text_score.text = _tmp.ToString();
    }

    public void Update_UI_Time(float _minutes, float _seconds)
    {
        text_time.text = string.Format("{0:00} : {1:00}", _minutes, _seconds);
    }
    
    public void Show_EndGame_UI()
    {
        foreach (var _tmp in objs_to_disable)
            _tmp.SetActive(false);

        panel_over.SetActive(true);
    }

    public void Create_EndGame_Holder(string _nickName, float _score)
    {
        var holder_ = Instantiate(holder_refab, info_place_holder);
        holder_.GetComponent<Info_Holder>().Set_Info(_nickName, _score);
    }
}