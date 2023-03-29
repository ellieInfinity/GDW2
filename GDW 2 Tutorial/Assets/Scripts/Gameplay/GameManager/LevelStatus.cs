using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStatus : MonoBehaviour
{
    bool levelCompleted = false;
    bool levelFailed = false;
    bool gameOver = false;

    public string _levelFailedScene;
    public string _levelCompleteScene;
    public string _gameOverScene;

    // Update is called once per frame
    void Update()
    {
        if (levelCompleted)
        {
            SceneManager.LoadScene(_levelCompleteScene);
        }

        if (levelFailed)
        {
            SceneManager.LoadScene(_levelFailedScene);
        }

        if (gameOver)
        {
            SceneManager.LoadScene(_gameOverScene);
        }
    }

    public void SetLevelComplete(bool complete)
    {
        levelCompleted = complete;
    }

    public void SetLevelFailed(bool failed)
    {
        levelFailed = failed;
    }

    public void SetGameOver(bool isGameOver)
    {
        gameOver = isGameOver;
    }
}
