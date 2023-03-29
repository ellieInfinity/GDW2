using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<CoinCounter>().AddCoin(1);
            FindObjectOfType<ScoreCounter>().AddScore(100);
            
            FindObjectOfType<AudioManager>().Play("Coin");
            Destroy(gameObject);
        }
    }
}
