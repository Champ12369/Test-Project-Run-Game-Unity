using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followplayer : MonoBehaviour
{
    public Transform target;  // The player's transform to follow
    public Vector3 offset;
    [Range(0f, 1f)]
    public float smoothfactor;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothfactor);
            transform.position = smoothPosition;
        }
    }
}
