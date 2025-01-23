using UnityEngine;

public class SpotlightFollow : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform player;
    private Vector3 velocity = Vector3.zero;

    #endregion

    #region Unity Methods

    private void Update()
    {
        Vector3 targetPosition = player.position + offset;

        //follow the target position with a smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    #endregion
}
