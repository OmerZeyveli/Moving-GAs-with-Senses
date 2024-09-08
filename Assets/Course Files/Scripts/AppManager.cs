using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{

    public static int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit.
            Application.Quit();
        }
    }

    void LoadNextLevel() // Load the spesific level given as parameter.
    {
        int currentSceneIndex = GetCurrentSceneIndex();
        int nextSceneIndex = currentSceneIndex + 1;
        // If index is larger than biggest scene num, load first scene.
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
