using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLives : MonoBehaviour
{
    [SerializeField] int initialLives = 5;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Lives", initialLives);
    }
}
