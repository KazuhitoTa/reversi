using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
     bool coroutineBool = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("right"))
         {
            if (!coroutineBool)
            {
                coroutineBool = true;
                StartCoroutine("RightMove");
            }
         }
       
    }

     IEnumerator RightMove()
    {
        for (int turn=0; turn<180; turn++)
        {
            transform.Rotate(1,0,0);
            yield return new WaitForSeconds(0.01f);
        }
        coroutineBool = false;
    }
}
