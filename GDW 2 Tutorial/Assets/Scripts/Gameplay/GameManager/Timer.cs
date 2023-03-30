using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] GameObject timerDisplay;

    int timeCount = 90000;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Countdown());

        if (Input.GetKey(KeyCode.P))
        {
            timeCount = 3000;
        }
    }

    IEnumerator Countdown()
    {
        if (timeCount > 0)
        {
            yield return new WaitForSeconds(1.0f);
            if (FindObjectOfType<LevelStatus>().GetLevelComplete() == false)
            {
                timeCount--;
                timerDisplay.GetComponent<NumberDisplayDefinition>()._numericValue = (timeCount/300).ToString();
            }
        }
        else if (timeCount <= 0) timeCount = 0;
    }

    public int GetTime()
    {
        return timeCount;
    }

    public void SetTime(int time)
    {
        timeCount = time;
    }
}
