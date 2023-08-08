using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using static GameManaging.GameState;

public class GameManaging : MonoBehaviour
{
    // class for managing game states
    public static class GameState
    {
        public enum GameStates
        {
            Playing, Paused, Busy, Battling
        };

        /// <summary>
        /// Enum for the current state of the game.
        /// </summary>
        public static GameStates currentState { get; private set; }

        /// <summary>
        /// Changes the state of the game to the specified state.
        /// </summary>
        public static void ChangeState(GameStates newState)
        {
            currentState = newState;
        }

        /// <summary>
        /// Checks if the current state of the game is the same as the state specified.
        /// </summary>
        public static bool IsState(GameStates checkState) 
        {
            return currentState == checkState;
        }
    }
}
