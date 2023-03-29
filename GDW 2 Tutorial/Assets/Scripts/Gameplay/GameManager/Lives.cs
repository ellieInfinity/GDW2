using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    [SerializeField] int lives = 5;
    [SerializeField] GameObject livesDisplay;

    int currentLives;

    // Update is called once per frame
    void Update()
    {
       currentLives = PlayerPrefs.GetInt("Lives");

       if (currentLives < 1)
       {
        GetComponent<LevelStatus>().SetGameOver(true);
       } 

       livesDisplay.GetComponent<NumberDisplayDefinition>()._numericValue = currentLives.ToString();
    }

    public void LoseLife()
    {
        currentLives--;
        PlayerPrefs.SetInt("Lives", currentLives);
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
