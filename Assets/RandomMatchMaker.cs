using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using osero;

public class RandomMatchMaker : MonoBehaviour//PunCallbacks
{
    // public GameObject Manager;
    // bool a;
    // void Start()
    // {
    //     PhotonNetwork.ConnectUsingSettings();
    // }
    // void Update()
    // {
    //     if(a)Manager.GetPhotonView().RPC("Link", RpcTarget.AllBuffered,Input.GetMouseButtonDown(0));
    // }

    // public override void OnConnectedToMaster()
    // {
    //     PhotonNetwork.JoinRandomRoom();
    //     Debug.Log("接続したで！");
    // }

    // public override void OnJoinedLobby()
    // {
    //     PhotonNetwork.JoinRandomRoom();
        
    // }

    // public override void OnJoinRandomFailed(short returnCode, string message)
    // {
    //     RoomOptions roomOptions = new RoomOptions();
    //     roomOptions.MaxPlayers = 2;
    //     PhotonNetwork.CreateRoom(null, roomOptions);
    // }

    // public override void OnJoinedRoom()
    // {
    //     a=true;
    // }
}