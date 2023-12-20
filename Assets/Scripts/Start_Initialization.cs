using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Zenject;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Start_Initialization : MonoBehaviourPunCallbacks
{
    [SerializeField] int room_size;

    [Inject] Canvas_Menu canvas_menu;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log("CONNECTED TO MASTERAS");
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 10000).ToString("0000");
        canvas_menu.Enable_UI();

        Debug.Log("CONNECTED TO LOBY");
    }

    #region ROOM SETTINGS
    public void Create_Room()
    {
        string room_name = Random.Range(0, 10000).ToString();

        RoomOptions room_settings =
        new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = room_size,
        };

        // hastable with players loaded | we ll use later when loaded on game sceme

        Hashtable hashTable_quantity = new Hashtable();
        hashTable_quantity.Add("players_quantity", 0);
        room_settings.CustomRoomProperties = hashTable_quantity;

        PhotonNetwork.CreateRoom(room_name, room_settings);
        canvas_menu.Show_UI_Match_Searching();
    }

    public override void OnJoinedRoom()
    {
        Count_And_Start();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create_Room();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Count_And_Start();
    }

    void Count_And_Start()
    {
        if (PhotonNetwork.PlayerList.Length >= room_size)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }

            PhotonNetwork.LoadLevel(1);

        }
    }

    public void Connect_Random_Room()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion
}