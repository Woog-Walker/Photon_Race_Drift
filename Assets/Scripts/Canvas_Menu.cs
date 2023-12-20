using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Canvas_Menu : MonoBehaviour
{
    [SerializeField] GameObject panel_menu;
    [SerializeField] GameObject panel_ccy;
    [SerializeField] TMP_Text text_loading;
    [Space]
    [SerializeField] GameObject button_start;
    [SerializeField] GameObject button_cancel;

    public void Enable_UI()
    {
        panel_menu.SetActive(true);
        panel_ccy.SetActive(true);
        text_loading.enabled = false;
    }

    public void Show_UI_Match_Searching()
    {
        button_start.SetActive(false);
        button_cancel.SetActive(true);
    }

    public void Show_UI_Searching_Cancel()
    {
        button_start.SetActive(true);
        button_cancel.SetActive(false);
    }
}