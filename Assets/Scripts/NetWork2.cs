using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetWork2 : MonoBehaviourPunCallbacks
{
    public GameObject player;
    public GameObject flagAcucar;
    public GameObject flagOutono;

    private void Start()
    {
        Login();
    }

    // ================================================
    // HELPERS
    // ================================================
    public void Login()
    {
        print("##################### LOGIN ##################");
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ================================================
    // PUN callbacks
    // ================================================

    public override void OnConnectedToMaster()
    {
        print("Connected to server");

        PhotonNetwork.JoinLobby();
        PhotonNetwork.NickName = "Gamer";
    }

    public override void OnJoinedLobby()
    {
        print("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("OnJoinRandomFailed: " + message);
        RoomOptions opt = new RoomOptions() { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom("Facens", opt);
    }

    public override void OnJoinedRoom()
    {
        print("OnJoinedRoom");

        print("Nome da sala: " + PhotonNetwork.CurrentRoom.Name);
        print("Players conectados: " + PhotonNetwork.CurrentRoom.PlayerCount);

        Hashtable myHash = new Hashtable();
        myHash.Add("score", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(myHash, null, null);

        CreatePlayer();
    }

    public void CreatePlayer()
    {
        PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity);
    }
    public void CreateFlagOutono()
    {
        PhotonNetwork.Instantiate(flagOutono.name, Vector3.zero, Quaternion.identity);
    }
    public void CreateFlagAcucar()
    {
        PhotonNetwork.Instantiate(flagAcucar.name, Vector3.zero, Quaternion.identity);
    }
}
