using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement; //シーン遷移させる場合に必要

namespace Photon.Chat.Demo{
public class PlayerController1 :Photon.Pun.MonoBehaviourPun
{
    public PhotonView tamesi;
    public GameObject tt;    
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;
    private float _horizontal;
    private float _vertical;
    private Quaternion _playerRotation; 
    public ChatGui test;
    public GameObject tmp;

    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        _playerRotation = _transform.rotation; 

        DontDestroyOnLoad(gameObject); //シーンを切り替えても削除しない

    }
    

    void FixedUpdate()
    {
        //if(!photonView.IsMine) return;   // ★追加
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

         if (_vertical >0)
        {            
            transform.position += transform.forward * _vertical/10f * _vertical;
            _animator.SetFloat("Speed",1.0f*_vertical);
        }
        else if (_vertical <0)
        {            
            transform.position += transform.forward * 0.02f * _vertical;
            _animator.SetFloat("Speed", -0.2f);
        }
        else
        {
            _animator.SetFloat("Speed", 0);
        }

        transform.Rotate(new Vector3(0, 3.0f * _horizontal, 0));
    }

    void OnCollisionEnter(Collision collision)
    {
        /*if(!photonView.IsMine)return;
        
        if(collision.gameObject.tag == "Cube")
        {
            if(SceneManager.GetActiveScene().name == "Scene_Lobby")//Lobbyなら
            {
                test.chatClient.Subscribe("Taiikukan".Split(new char[] {' ', ','}));
                test.chatClient.Unsubscribe("Lobby".Split(new char[] {' ', ','}));
                SceneManager.LoadScene("Scene_Taiikukan");
                transform.position = new Vector3(996, 1, 10);
                tt.transform.position = new Vector3(996, 1, 10);
            }   
            else
            {   
                test.chatClient.Subscribe("Lobby".Split(new char[] {' ', ','}));
                test.chatClient.Unsubscribe("Taiikukan".Split(new char[] {' ', ','}));
                SceneManager.LoadScene("Scene_Lobby");
                transform.position = new Vector3(-4, 1, 10);
                tt.transform.position = new Vector3(-4, 1, 10);
            }
        }*/       
    }
    }

}
//自分キャラの名前を変更してね(オフライン)
//他のキャラの名前を取得してね