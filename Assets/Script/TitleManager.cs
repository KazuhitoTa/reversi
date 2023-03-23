using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{
    bool onStart;
    public void OnLineStartBotton()
    {
        if(onStart)return;
        GameDateManager.Instance.InOnlineBattle=true;
        
        onStart=true;
        SceneManager.LoadScene("Online");

    }
    public void CPUStartBotton()
    {
        if(onStart)return;
        GameDateManager.Instance.InCPUBattle=true;
        
        onStart=true;
        SceneManager.LoadScene("Game");

    }
}
