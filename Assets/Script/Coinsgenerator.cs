using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coinsgenerator : MonoBehaviour
{
    public GameObject[] coins;
    private void OnEnable()
    {
        if(coins.Length!=0)
        {

            for (int i = 0; i < coins.Length; i++)
            {
                coins[i].SetActive(true);

            }

        }

        
    }
}
