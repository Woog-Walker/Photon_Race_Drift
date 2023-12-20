using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public class Drift_Score : MonoBehaviourPunCallbacks
{
    [SerializeField] float current_score;
    [SerializeField] float score_per_drift;
    [HideInInspector] public bool is_drifting = false;
    [Space]
    [SerializeField] Canvas_InGame_Manager canvas_ingame_screen;
    [SerializeField] Canvas_InGame_World canvas_ingame_world;

    float current_drift_score = 0;

    private void FixedUpdate()
    {
        if (is_drifting)
            Add_Score();
    }

    public void Add_Score()
    {
        current_drift_score += score_per_drift;

        canvas_ingame_world.Update_Score_Text(current_drift_score);
    }

    public void Drift_Start()
    {
        is_drifting = true;

        canvas_ingame_world.Score_Tab_Enable();
    }

    public void Drift_End()
    {
        is_drifting = false;

        current_score += current_drift_score;
        canvas_ingame_screen.Update_UI_Score(current_score);

        Save_Score_To_Hash();

        canvas_ingame_world.Score_Tab_Disable();

        current_drift_score = 0;
    }

    void Save_Score_To_Hash()
    {
        Hashtable drift_hash = new Hashtable();
        drift_hash.Add("drift_score", current_score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(drift_hash);
    }

    public void Fill_EndGame_Info()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Hashtable custom_properties = player.CustomProperties;
            string tmp_nick = (player.NickName) as string;
            float tmp_score = (float)player.CustomProperties["drift_score"];

            canvas_ingame_screen.Create_EndGame_Holder(tmp_nick, tmp_score);
        }
    }
}

/*
 *     public float Get_Player_Score()
    {
        float end_game_score = (float)PhotonNetwork.LocalPlayer.CustomProperties["drift_score"];

        return end_game_score;
    }

    public string Get_Player_Nick()
    {
        string player_nick = (PhotonNetwork.LocalPlayer.NickName) as string;

        return player_nick;
    }
*/