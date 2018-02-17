using UnityEngine;
using UnityEngine.SceneManagement;

// just pasted from the internet
static public class MoveObjectToNewScene
{
    static GameObject[] targetObjects;
    static string targetSceneName;
    static Scene currentScene;
    static Scene newScene;
    static bool shouldLoadNewScene = false;

    /// <summary>
    /// Move a GameObject from the current scene to another scene.
    /// </summary>
    /// <param name="sceneName">Name of the scene you want to load.</param>
    /// <param name="targetGameObjects">GameObject you want to move to the new scene.</param>
    static public void LoadScene(string sceneName, GameObject[] targetGameObjects, float sequenceDur = 0)
    {
        Game.newSceneTimer = sequenceDur;
        // set some globals
        targetObjects = targetGameObjects;
        targetSceneName = sceneName;

        // get the current active scene
        currentScene = SceneManager.GetActiveScene();

        // load the new scene in the background
        SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);

        // Attach the SceneLoaded method to the sceneLoaded delegate.
        // SceneLoaded will be called when the new scene is loaded.
        SceneManager.sceneLoaded += SceneLoaded;
    }

    /// <summary>
    /// After new scene loads, move GameObject from current scene to new scene.
    /// When finished, unload current scene. The new scene becomes current scene.
    /// </summary>
    /// <param name="newScene">New scene that was loaded.</param>
    /// <param name="loadMode">Mode that was used to load the scene.</param>
    static public void SceneLoaded(Scene newScene, LoadSceneMode loadMode)
    {
        shouldLoadNewScene = true;
    }

    static public void GoToLoadedScene()
    {
        if (shouldLoadNewScene)
        {
            shouldLoadNewScene = false;
            // remove this method from the sceneLoaded delegate
            SceneManager.sceneLoaded -= SceneLoaded;

            // get the scene we just loaded into the background
            newScene = SceneManager.GetSceneByName(targetSceneName);

            // move the gameobjects from scene A to scene B
            foreach (GameObject go in targetObjects)
                SceneManager.MoveGameObjectToScene(go, newScene);

            // unload scene A
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}