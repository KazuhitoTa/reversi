using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDManager : MonoBehaviour
{
    public bool PlayerCheck;
    public GameObject[] Camera;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerCheck)
        {
            Camera[0].SetActive(true);
            Camera[1].SetActive(false);
        }
        else
        {
            Camera[0].SetActive(false);
            Camera[1].SetActive(true);   
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
