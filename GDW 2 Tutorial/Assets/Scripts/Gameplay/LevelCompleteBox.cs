using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteBox : MonoBehaviour
{
    [SerializeField] List<Sprite> powerupList;

    float changeTimer = 0;
    int previousChoice = 5;

    bool itemCollected;

    // Update is called once per frame
    void Update()
    {
        if (!itemCollected) ChangeImage();
    }

    void ChangeImage()
    {
        if (changeTimer < Time.realtimeSinceStartup)
        {
            int choice = (int)Random.Range(0, powerupList.Count);

            if (choice == previousChoice)
            {
                ChangeImage();
                return;
            }

            GetComponent<SpriteRenderer>().sprite = powerupList[choice];

            changeTimer = Time.realtimeSinceStartup + 0.15f;

            previousChoice = choice;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        itemCollected = true;

        FindObjectOfType<AudioManager>().Stop("Music");
        FindObjectOfType<AudioManager>().Play("LevelClear");

        FindObjectOfType<ScoreCounter>().AddScore((FindObjectOfType<Timer>().GetTime()/300) * 50);

        FindObjectOfType<Timer>().SetTime(0);

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime (1.5f);

        FindObjectOfType<LevelStatus>().SetLevelComplete(true);
    }

    public bool GetItemCollected()
    {
        return itemCollected;
    }
}
