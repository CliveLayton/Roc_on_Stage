using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
   #region Variables

   private GameInput gameInput;
   private PlayerController playerController;

   #endregion

   #region Unity Methods

   // We create a new PlayerControllerMap
   private void Awake()
   {
      gameInput = new GameInput();
      playerController = GetComponent<PlayerController>();
   }

   /// <summary>
   /// Enable the PlayerControllerMap.
   /// Subscribe methods to certain buttons. 
   /// </summary>
   private void OnEnable()
   {
      gameInput.Enable();

      gameInput.Player.Move.performed += playerController.OnMove;
      gameInput.Player.Move.canceled += playerController.OnMove;

      gameInput.Player.Jump.performed += playerController.OnJump;

      gameInput.Player.Sprint.performed += playerController.OnSprint;
      gameInput.Player.Sprint.canceled += playerController.OnSprint;

      gameInput.Player.Attack.performed += playerController.OnAttacking;

      gameInput.Player.Counter.performed += playerController.OnCounter;
   }

   /// <summary>
   /// Disable the PlayerControllerMap.
   /// Desubscribe methods to certain buttons. 
   /// </summary>
   private void OnDisable()
   {
      gameInput.Disable();
      
      gameInput.Player.Move.performed -= playerController.OnMove;
      gameInput.Player.Move.canceled -= playerController.OnMove;
      
      gameInput.Player.Jump.performed -= playerController.OnJump;

      gameInput.Player.Sprint.performed -= playerController.OnSprint;
      gameInput.Player.Sprint.canceled -= playerController.OnSprint;

      gameInput.Player.Attack.performed -= playerController.OnAttacking;

      gameInput.Player.Counter.performed -= playerController.OnCounter;
   }

   #endregion
}
