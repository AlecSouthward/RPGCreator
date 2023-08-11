using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManaging : MonoBehaviour
{
    // class for managing rooms
    public static class Room
    {
        public static string RoomName { get; private set; }

        /// <summary>
        /// The current room's scene build index.
        /// </summary>
        public static int RoomNum { get; private set; }

        public static Scene RoomScene { get; private set; }

        /// <summary>
        /// Unloads the old room scene and will load the specified scene (by string) asynchronously.
        /// </summary>
        /// <param name="newRoomName">String for the new room's scene. Scene must be in build settings.</param>
        public static IEnumerator ChangeRoom(string newRoomName, Vector2 playerNewPos)
        {
            // sets the game's state to busy, so the player
            // cannot move while loading
            GameState.ChangeState(GameState.States.Busy);

            // TODO: transition animation between loading of rooms

            // ! TODO !: if the newRoomName is an invalid scene name,
            // an exception will be thrown. Instead make it load a default room.

            // starts loading the new room's scene
            AsyncOperation newSceneLoad = SceneManager.LoadSceneAsync(
                newRoomName, LoadSceneMode.Additive);

            if (RoomScene.isLoaded)
            {
                // starts unloading the old room's scene
                AsyncOperation oldSceneUnload = SceneManager.UnloadSceneAsync(RoomNum);

                // waits for the old scene to unload
                // and the new scene to load
                yield return new WaitUntil(() =>
                {
                    return newSceneLoad.isDone && oldSceneUnload.isDone;
                });

                Debug.Log("Switched to new room: " + newRoomName + ", from room: " + RoomName);
            }
            else
            {
                // waits for the new scene to load
                yield return new WaitUntil(() =>
                {
                    return newSceneLoad.isDone;
                });

                Debug.Log("Loaded new room: " + newRoomName);
            }

            // sets the old room variables to the new one
            RoomScene = SceneManager.GetSceneByName(newRoomName);

            // sets the player position to the new position
            PlayerController.instance.transform.position = playerNewPos;

            RoomName = RoomScene.name;
            RoomNum = RoomScene.buildIndex;

            // resets the game's state to playing
            GameState.ChangeState(GameState.States.Playing);
        }
    }

    // class for managing game states
    public static class GameState
    {
        public enum States
        {
            Playing, Paused, Busy, Battling
        };

        /// <summary>
        /// Enum for the current state of the game.
        /// </summary>
        public static States CurrentState { get; private set; }

        /// <summary>
        /// Changes the state of the game to the specified state.
        /// </summary>
        public static void ChangeState(States newState)
        {
            CurrentState = newState;
        }

        /// <summary>
        /// Checks if the current state of the game is the same as the state specified.
        /// </summary>
        public static bool IsState(States checkState) 
        {
            return CurrentState == checkState;
        }
    }
}