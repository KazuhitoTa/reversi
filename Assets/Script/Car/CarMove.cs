using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarMove :Photon.Pun.MonoBehaviourPun
{
    public AudioSource engine;
    private float _horizontal;
    private float _vertical;

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) return;   // ★追加
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

         if (_vertical >0)
        {         
            engine.Play();   
            transform.position += transform.forward * _vertical * _vertical*-10*Time.deltaTime;
            transform.Rotate(new Vector3(0, 100f * _horizontal*Time.deltaTime, 0));
        }
        else if (_vertical <0)
        {    
            engine.Play();         
            transform.position += transform.forward *_vertical*-5*Time.deltaTime;
            transform.Rotate(new Vector3(0, 100f * _horizontal*Time.deltaTime, 0));
        }
        else
        {
            engine.Pause();
        }

        
    }
}
