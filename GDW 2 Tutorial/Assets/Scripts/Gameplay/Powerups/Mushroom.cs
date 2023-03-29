using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    bool isMoving = false;
    float speed = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if (isMoving) transform.position += Vector3.left * Time.deltaTime * speed;
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        isMoving = true;
    }
}
