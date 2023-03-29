using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    [SerializeField] float kickForce = 2.5f;
    [SerializeField] float speed = 1.5f;

    bool isSquashed;
    bool isKicked;
    bool isMoving = true;
    bool movingLeft;

    void Update()
    {
        if (!isSquashed)
        {
            Move();
        }
    }

    void Move()
    {
        if (movingLeft)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(0, 0 ,0);
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(0, 180 ,0);
        }

        isMoving = true;
    }

    public void ApplyKickForce(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * kickForce, ForceMode2D.Impulse);
        isKicked = true;
        isMoving = true;

        FindObjectOfType<AudioManager>().Play("Kick");
    }

    public bool GetIsSquashed ()
    {
        return isSquashed;
    }

    public void SetIsSquashed (bool squashed)
    {
        isSquashed = squashed;
    }

    public bool GetIsKicked ()
    {
        return isKicked;
    }

    public void SetIsKicked (bool kicked)
    {
        isKicked = kicked;
    }

    public bool GetIsMoving ()
    {
        return isMoving;
    }

    public void SetIsMoving (bool moving)
    {
        isMoving = moving;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isSquashed)
        {
            movingLeft = !movingLeft;
        }

        if (isKicked)
        {
            if (other.contacts[0].normal.x > 0)
            {
                ApplyKickForce(new Vector2(1,0));
            }
            if (other.contacts[0].normal.x < 0)
            {
                ApplyKickForce(new Vector2(-1,0));
            }

            if (other.gameObject.tag == "Goomba" || other.gameObject.tag == "PPlant")
            {
                other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;

                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8, ForceMode2D.Impulse);

                other.gameObject.GetComponent<Collider2D>().enabled = false;

                Destroy(other.gameObject, 2);

                FindObjectOfType<AudioManager>().Play("Bump");

                ApplyKickForce( new Vector2(-other.contacts[0].normal.normalized.x, 0));
            }
        }

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
