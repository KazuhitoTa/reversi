using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDateManager : MonoBehaviour
{
    //オンライン戦かどうか
    public bool InOnlineBattle{get;set;}
    public bool InCPUBattle{get;set;}

    //シーンをまたいでも破壊されない
    public static GameDateManager Instance{get; private set;}
    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
