using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlant : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 1f;
    public float pauseTime = 2f;
    bool isDead = false;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        if (!isDead)
        {
            StartCoroutine(MoveObject());
        } 
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            yield return StartCoroutine(MoveDown());
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(MoveUp());
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator MoveDown()
    {
        float time = 0f;
        Vector3 endPos = startPos - new Vector3(0f, moveDistance, 0f);

        while (time < pauseTime)
        {
            time += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, time);
            yield return null;
        }
        transform.position = endPos;
    }

    IEnumerator MoveUp()
    {
        float time = 0f;
        Vector3 endPos = startPos;

        while (time < pauseTime)
        {
            time += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos - new Vector3(0f, moveDistance, 0f), endPos, time);
            yield return null;
        }
        transform.position = endPos;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "MarioFire")
        {
            isDead = true;

            Destroy(other.gameObject);

            FindObjectOfType<ScoreCounter>().AddScore(1000);

            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;

            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8, ForceMode2D.Impulse);

            gameObject.GetComponent<Collider2D>().enabled = false;

            Destroy(gameObject, 1f);
        }
    }
}
