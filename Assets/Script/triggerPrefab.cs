using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerPrefab : MonoBehaviour
{
 public BackgroundGenerator backgroundGenerator;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {   
            backgroundGenerator.GeneratePrefab();

    
        }


    }
}
