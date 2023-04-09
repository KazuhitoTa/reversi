using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using osero;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    bool inRoom;
    bool isMatching;
    public void OnMatchingButton() 
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    /// <summary>
    /// 現在ゲームを遊んでいるプレイヤーの総数や、ゲームサーバーに作成されている全てのルームの情報を監視しています。
    /// ここで新しいルームを作成したり、既に存在するルームへランダムで参加したり、
    /// またはロビーに参加して、既に存在するルーム一覧から参加するルームを選んだりすることができます。
    /// プレイヤーはルームへ参加する時に、そのルームが作成されているゲームサーバーへ転送されます。
    /// </summary>
    public override void OnConnectedToMaster() 
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    /// <summary>
    /// ルームは全てゲームサーバーに作成されます。ルームへ参加したプレイヤーは、
    /// ゲームサーバーを通して同じルームへ参加している他プレイヤーとデータを送受信して、
    /// ゲームの状態を同期させていきます。ここでプレイヤーがルームから退出した時は、
    /// 元のマスターサーバーへ再び転送されます。
    /// </summary>
    public override void OnJoinedRoom() 
    {
        inRoom=true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions(){MaxPlayers=2},TypedLobby.Default);
    }

    private void Update()
    {
        
        if(isMatching)return;
        if(inRoom)
        {
            if(PhotonNetwork.CurrentRoom.MaxPlayers==PhotonNetwork.CurrentRoom.PlayerCount)
            {
                isMatching=true;
                SceneManager.LoadScene("Game");
            }
        }
        
    }
}