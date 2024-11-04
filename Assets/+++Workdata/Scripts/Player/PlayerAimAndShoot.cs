using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private GameObject bulletInst;

    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle = 0f;

    private bool isShooting;

    #endregion

    #region Unity Methods

    private void Update()
    {
        HandleGunRotation();
        HandleGunShooting();
    }

    #endregion

    #region Input Methods

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isShooting = true;
        }

        if (context.canceled)
        {
            isShooting = false;
        }
    }

    #endregion

    #region Gun Methdos

    private void HandleGunRotation()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        
        angle = Mathf.Clamp(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, -90, 90);
        
        if (angle < 90 && angle > -90)
        {
            //rotate the gun towards the mouse position
            gun.transform.right = direction;
        }

        
        //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
    }

    private void HandleGunShooting()
    {
        if (isShooting)
        {
            bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
        }
    }

    #endregion
    
}
