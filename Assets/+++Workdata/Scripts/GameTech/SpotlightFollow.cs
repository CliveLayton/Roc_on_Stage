using System;
using UnityEngine;

public class SpotlightFollow : MonoBehaviour
{
    #region Variables
    
    public Vector3 offset;
    public float smoothTime;
    public Transform player;
    private Vector3 velocity = Vector3.zero;

    #endregion

    #region Unity Methods

    private void Update()
    {
        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    #endregion
}
