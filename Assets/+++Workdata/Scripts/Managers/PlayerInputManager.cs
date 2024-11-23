using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
   #region Variables

   private GameInput gameInput;
   private PlayerController playerController;
   private PlayerAimAndShoot playerAimAndShoot;

   #endregion

   #region Unity Methods

   // We create a new PlayerControllerMap and get the PlayerJumping, PlayerMovement and PlayerObjectMove. 
   private void Awake()
   {
      gameInput = new GameInput();
      playerController = GetComponent<PlayerController>();
      playerAimAndShoot = GetComponent<PlayerAimAndShoot>();
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

      gameInput.Player.DodgeRoll.performed += playerController.OnDodgeRoll;

      gameInput.Player.StompAttack.performed += playerController.OnStompAttack;

      gameInput.Player.Shoot.performed += playerAimAndShoot.OnShoot;
      gameInput.Player.Shoot.canceled += playerAimAndShoot.OnShoot;
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

      gameInput.Player.DodgeRoll.performed -= playerController.OnDodgeRoll;
      
      gameInput.Player.StompAttack.performed -= playerController.OnStompAttack;

      gameInput.Player.Shoot.performed -= playerAimAndShoot.OnShoot;
      gameInput.Player.Shoot.canceled -= playerAimAndShoot.OnShoot;
   }

   #endregion
}
