using TrixieGames.Maze.Managers;
using UnityEngine;

namespace TrixieGames.Maze.Controllers
{
    [DefaultExecutionOrder(-100)]
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField]
        private InputManager m_InputManager;

        //Create Variables to hold input Updates to access elsewhere here

        private void Awake()
        {
            m_InputManager.m_AttackEvent += HandleWeakAttackEvent;
            m_InputManager.m_MoveEvent += HandleMoveEvent;
            m_InputManager.m_PauseEvent += HandlePauseEvent;
        }

        //Set the variables in the Events below

        private void HandlePauseEvent()
        {
            Debug.LogError("TODO: Implement PlayerInput.HandlePauseEvent()");
        }

        private void HandleMoveEvent(float _direction)
        {
            Debug.LogError("TODO: Implement PlayerInput.HandleMoveEvent()");
        }

        private void HandleWeakAttackEvent()
        {
            Debug.LogError("TODO: Implement PlayerInput.HandleWeakAttackEvent()");
        }

    }
}
