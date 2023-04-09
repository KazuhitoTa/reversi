using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RandomMatchMaker : MonoBehaviourPunCallbacks
{
    // インスペクターから設定
    public GameObject PhotonObject;
    private GameObject player;
    
    void Awake() 
    {
        Application.targetFrameRate = 60; //60FPSに設定
        Debug.Log(Application.targetFrameRate);
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        player=PhotonNetwork.Instantiate(PhotonObject.name,new Vector3(0f, 1f, 0f),Quaternion.identity,0);
        Transform tmp=player.transform.Find("CamPos");
        Car.CarCamera.target=tmp;
        GameObject maincamera = GameObject.FindWithTag("MainCamera");
        maincamera.GetComponent<Car.CarCamera>().enabled = true;
    }
}