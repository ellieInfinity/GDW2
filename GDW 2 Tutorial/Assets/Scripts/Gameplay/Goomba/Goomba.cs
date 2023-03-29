using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    [SerializeField] float deathTimer = 0.2f;

    bool isSquashed;
    bool movingLeft;

    float flipTimer = 0;
    float speed = 1.5f;

    void Update()
    {
        if (!isSquashed)
        {
            Move();
        }

        if (isSquashed)
        {
            Destroy(gameObject, deathTimer);
        }

        if (flipTimer <= Time.realtimeSinceStartup)
        {
            transform.Rotate(new Vector3(0, 1, 0), 180);
            flipTimer = Time.realtimeSinceStartup + 0.25f;
        }
    }

    void Move()
    {
        if (movingLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
    }

    public bool GetIsSquashed ()
    {
        return isSquashed;
    }

    public void SetIsSquashed (bool squashed)
    {
        isSquashed = squashed;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        movingLeft = !movingLeft;

        if (other.gameObject.tag == "MarioFire")
        {
            Destroy(other.gameObject);

            FindObjectOfType<ScoreCounter>().AddScore(1000);

            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;

            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8, ForceMode2D.Impulse);

            gameObject.GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 2);
        }
    }
}
