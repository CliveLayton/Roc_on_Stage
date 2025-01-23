using UnityEngine;
using UnityEngine.InputSystem;

//currently not in use 
public class PlayerAimAndShoot : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    public float timeBetweenBullets = 0.5f;
    public float reloadTime;

    private GameObject bulletInst;
    private PlayerController playerController;

    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle = 0f;
    private float timeSinceLastBullet;

    private bool isShooting = false;
    private bool isFacingRight = true;
    private Vector3 bulletDirection;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        HandleGunRotation();

        timeSinceLastBullet += Time.deltaTime;
        
        if (!isShooting)
        {
            return;
        }
        
        if (timeSinceLastBullet < timeBetweenBullets)
        {
            return;
        }

        if (CheckForTarget())
        {
            timeSinceLastBullet = 0;
            HandleGunShooting();
        }
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

    #region Gun Methods

    /// <summary>
    /// rotate the gun to the cursor
    /// </summary>
    private void HandleGunRotation()
    {
        if (playerController.moveInput.x > 0)
        {
            isFacingRight = true;
        }
        else if (playerController.moveInput.x < 0)
        {
            isFacingRight = false;
        }
        
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        
        if (isFacingRight)
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            if (angle < 80 && angle > -80)
            {
                //rotate the gun towards the mouse position
                gun.transform.right = direction;
            }
        }
        else if (!isFacingRight)
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle > 120 || angle < -120)
            {
                //rotate the gun towards the mouse position
                gun.transform.right = direction;
            }

        }

        //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
    }

    /// <summary>
    /// instantiate a bullet and set direction for it
    /// </summary>
    private void HandleGunShooting()
    {
        bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.collider.CompareTag("Enemy"))
        { 
            Vector3 bulletTarget = hit.transform.position;
            bulletDirection = (bulletTarget - gun.transform.position).normalized;
        }
        bulletInst.GetComponent<BulletBehavior>().SetDirection(bulletDirection);
    }

    /// <summary>
    /// check if the cursor is on a enemy
    /// </summary>
    /// <returns>returns the enemy</returns>
    private bool CheckForTarget()
    {
        return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) && hit.collider.CompareTag("Enemy");
    }

    #endregion
    
}
