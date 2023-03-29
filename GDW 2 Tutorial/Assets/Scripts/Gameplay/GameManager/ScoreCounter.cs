using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] GameObject scoreDisplay;

    int scoreCount = 0;

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.GetComponent<NumberDisplayDefinition>()._numericValue = scoreCount.ToString();
    }

    public int GetScore()
    {
        return scoreCount;
    }

    public void AddScore (int amount)
    {
        scoreCount += amount;
    }

    public void RemoveScore (int amount)
    {
        scoreCount -= amount;
    }
}
