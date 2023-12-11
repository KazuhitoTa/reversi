using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
   [SerializeField] Collider goalCollision;
   [SerializeField] Collider carCollision;
   private bool goalReached;
   private int lapCount=0;

    void Update()
    {
        if(carCollision==null)
        {
            var temp=GameObject.FindGameObjectsWithTag("Player");
            carCollision=temp[0].GetComponent<Collider>();
        }
        

        if (!goalReached && goalCollision.bounds.Contains(carCollision.bounds.center))
        {
            goalReached = true;
            // ゴールエリア内に車がある場合の処理をここに記述する
            Debug.Log(lapCount);
            lapCount++;
        }
    }

}
