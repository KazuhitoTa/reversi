using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarMove :Photon.Pun.MonoBehaviourPun
{
    public AudioSource engine;
    private float _horizontal;
    private float _vertical;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine) return;   // ★追加
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

         if (_vertical >0)
        {         
            engine.Play();   
            transform.position += transform.forward * _vertical/100f * _vertical*-1;
        }
        else if (_vertical <0)
        {    
            engine.Play();         
            transform.position += transform.forward * 0.02f * _vertical*-1;
        }
        else
        {
            engine.Pause();
        }

        transform.Rotate(new Vector3(0, 0.2f * _horizontal, 0));
    }
}
