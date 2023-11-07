using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walktest : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    void Update()
    {
        // Move the player forward along its local forward axis
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
