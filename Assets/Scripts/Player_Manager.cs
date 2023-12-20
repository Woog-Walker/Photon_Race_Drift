using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Player_Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject player_prefab;
    [SerializeField] Level_Spawn_Points spawn_points;

    [Space]
    [Header("TMP ITEMS ON START")]
    [SerializeField] GameObject start_camera;
    [SerializeField] GameObject start_canva;

    private void Start()
    {
        Hashtable is_loaded = new Hashtable();
        is_loaded.Add("is_player_loaded", (int)1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(is_loaded);

        StartCoroutine(Wait_For_Players_Delay());
    }

    IEnumerator Wait_For_Players_Delay()
    {
        yield return new WaitForSeconds(2);

        int players_loaded = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Hashtable customProperties = player.CustomProperties;

            if (customProperties.ContainsKey("is_player_loaded"))
                players_loaded += (int)player.CustomProperties["is_player_loaded"];
        }

        if (players_loaded == PhotonNetwork.CurrentRoom.PlayerCount)
            Spawn_Player();
        else
            StartCoroutine(Wait_For_Players_Delay());
    }

    void Spawn_Player()
    {
        Destroy(start_canva);
        Destroy(start_camera);

        if (PhotonNetwork.InRoom)
        {
            int currentPlayerIndex = GetCurrentPlayerIndex();
            Transform spawn_place = spawn_points.Get_Point(currentPlayerIndex);

            PhotonNetwork.Instantiate
                (player_prefab.name,
                spawn_place.position,
                Quaternion.Euler(0, spawn_place.localEulerAngles.y, 0));
        }
    }

    int GetCurrentPlayerIndex()
    {
        Player[] players = PhotonNetwork.PlayerList;
        Player localPlayer = PhotonNetwork.LocalPlayer;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == localPlayer)
                return i;
        }

        return -1;
    }
}