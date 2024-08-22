using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Maze.Input;

using static Maze.Input.PlayerInputActions;

namespace Maze.Managers
{
    [CreateAssetMenu(menuName = "InputManager")]
    [DefaultExecutionOrder(-100)]
    public class InputManager : ScriptableObject, IPlayerControlActions
    {
        private PlayerInputActions playerInput;
        public PlayerInputActions PlayerControls { get { return playerInput; } }

        public event Action m_AttackEvent;
        public event Action<float> m_MoveEvent;
        public event Action m_JumpEvent;
        public event Action m_RollEvent;
        public event Action m_PauseEvent;

        private void OnEnable()
        {
            if (playerInput == null)
            {
                playerInput = new PlayerInputActions();
                playerInput.PlayerControl.SetCallbacks(instance: this);
                playerInput.PlayerControl.Enable();
            }
        }

        public void OnWeakAttack(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnStrongAttack(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnDefend(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnPlayerMovement(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnCameraMovement(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnLockCamera(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnAction(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}
