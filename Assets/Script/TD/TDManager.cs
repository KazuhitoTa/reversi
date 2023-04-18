using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDManager : MonoBehaviour
{
    public bool PlayerCheck;
    public GameObject[] camera;
    public GameObject[] backGrand;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    public void PutButton()
    {
        
        PlayerCheck=PlayerCheck?false:true;
        
        if(PlayerCheck)
        {
            camera[0].SetActive(true);
            camera[1].SetActive(false);
            backGrand[0].SetActive(true);
            backGrand[1].SetActive(false);
        }
        else
        {
            camera[0].SetActive(false);
            camera[1].SetActive(true);
            backGrand[0].SetActive(false);
            backGrand[1].SetActive(true);   
        }
    }
}
