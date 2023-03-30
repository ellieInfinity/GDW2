using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioController : MonoBehaviour
{
    [SerializeField] float runForce;
    [SerializeField] float jumpForce;
    [SerializeField] float maxSpeed;
    [SerializeField] Vector2 fireForce = new Vector2(25, 0);
    [SerializeField] private float shootingDelay = 0.5f;

    [SerializeField] GameObject bigMarioPrefab;
    [SerializeField] GameObject smolMarioPrefab;

    [SerializeField] GameObject firePrefab;
    [SerializeField] Transform fireParent;

    [SerializeField] float timeSinceLastShot;

    Transform trans;
    Rigidbody2D body;

    float runInput;
    int playerDirection;
    bool jumpInput;

    bool isGrounded;
    bool isBig;
    bool isDead;
    bool canShoot;
    bool isRunning;
    
    bool deathStarted;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        body = GetComponent<Rigidbody2D>();

        FindObjectOfType<AudioManager>().Play("Music");
    }

    // Update is called once per frame
    void Update()
    {
        runInput = Input.GetAxis("Horizontal");

        if (runInput == 0)
        {
            isRunning = false;
        }
        else
        {
            isRunning = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }

        if (runInput == 0 && body.velocity.y == 0)
        {
            body.drag = 3;
        }
        else
        {
            body.drag = 1;
        }

        if (trans.position.y <= -5 && !deathStarted)
        {
            StartDeath();
        }

        if (trans.position.y <= -7)
        {
            Die();
        }

        if (canShoot)
        {
            Shoot();
        }

        if (FindObjectOfType<Timer>().GetTime() <= 0)
        {
            if (FindObjectOfType<LevelCompleteBox>().GetItemCollected() == false)
            {
                StartDeath();
            }
            
        }
    }

    void FixedUpdate() 
    {
        if (runInput != 0)
        {
            Run();
        }

        if (jumpInput && isGrounded)
        {
            Jump();
        }
    }

    void Run()
    {
        isRunning = false;

        if (Mathf.Abs(body.velocity.x) >= maxSpeed)
        {
            return;
        }

        if (runInput > 0)
        {
            body.AddForce(Vector2.right * runForce, ForceMode2D.Force);
            trans.rotation = Quaternion.Euler(0,180,0);
            
            playerDirection = 1;
        }

        if (runInput < 0)
        {
            body.AddForce(Vector2.left * runForce, ForceMode2D.Force);
            trans.rotation = Quaternion.Euler(0,0,0);

            playerDirection = -1;
        }

    }

    void Jump()
    {
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;

        FindObjectOfType<AudioManager>().Play("Jump");
    }

    void EnemyBounce()
    {
        body.AddForce(Vector2.up * jumpForce / 1.5f, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void StartDeath()
    {
        FindObjectOfType<Lives>().LoseLife();

        FindObjectOfType<AudioManager>().Stop("Music");

        if (FindObjectOfType<Lives>().GetCurrentLives() < 1)
        {
            FindObjectOfType<AudioManager>().Play("GameOver");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("LifeLost");
        }

        isDead = true;

        body.velocity = Vector2.zero;

        body.gravityScale = 3;

        body.AddForce(Vector3.up * jumpForce/2, ForceMode2D.Impulse);
        
        GetComponent<Collider2D>().enabled = false;

        deathStarted = true;
    }

    void Die()
    {
        if (FindObjectOfType<Lives>().GetCurrentLives() < 1)
        {
            FindObjectOfType<LevelStatus>().SetGameOver(true);
        }

        else
        {
            FindObjectOfType<LevelStatus>().SetLevelFailed(true);
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && timeSinceLastShot > shootingDelay) 
        {
            var fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
            fire.transform.parent = fireParent;

            FindObjectOfType<AudioManager>().Play("Fireball");

            var forceOnBullet = fireForce * playerDirection;
            fire.GetComponent<Rigidbody2D>().AddForce(forceOnBullet, ForceMode2D.Impulse);

            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.5)
            {
                isGrounded = true;
            }
        }

        if (collision.gameObject.tag == "Goomba")
        {
            if (collision.contacts[0].normal.y > 0.5)
            {
                EnemyBounce();
                collision.gameObject.GetComponent<Goomba>().SetIsSquashed(true);

                FindObjectOfType<ScoreCounter>().AddScore(1000);

                FindObjectOfType<AudioManager>().Play("Bump");
            }
            else
            {
                if (!collision.gameObject.GetComponent<Goomba>().GetIsSquashed())
                {
                    if (isBig)
                    {
                        if (canShoot)
                        {
                            canShoot = false;

                            FindObjectOfType<AudioManager>().Play("PowerDown");
                        }

                        else
                        {
                            GetComponent<BoxCollider2D>().size = smolMarioPrefab.GetComponent<BoxCollider2D>().size;

                            isBig = false;

                            FindObjectOfType<AudioManager>().Play("PowerDown");
                        }
                    }
                    else
                    {
                        StartDeath();
                    }
                }
            }
        }

        if (collision.gameObject.tag == "Koopa")
        {
            if (collision.contacts[0].normal.y > 0.5 && !collision.gameObject.GetComponent<Koopa>().GetIsSquashed())
            {
                EnemyBounce();
                collision.gameObject.GetComponent<Koopa>().SetIsSquashed(true);
                collision.gameObject.GetComponent<Koopa>().SetIsMoving(false);

                collision.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1,1);

                FindObjectOfType<ScoreCounter>().AddScore(1000);

                FindObjectOfType<AudioManager>().Play("Bump");
            }

            else if (collision.gameObject.GetComponent<Koopa>().GetIsSquashed() && !collision.gameObject.GetComponent<Koopa>().GetIsKicked())
            {
                if (collision.gameObject.transform.position.x > trans.position.x)
                {
                    collision.gameObject.GetComponent<Koopa>().ApplyKickForce(new Vector2(1,0));
                }
                if (collision.gameObject.transform.position.x < trans.position.x)
                {
                    collision.gameObject.GetComponent<Koopa>().ApplyKickForce(new Vector2(-1,0));
                }
            }

            else if (collision.contacts[0].normal.y > 0.5 && collision.gameObject.GetComponent<Koopa>().GetIsKicked())
            {
                EnemyBounce();
                collision.gameObject.GetComponent<Koopa>().SetIsKicked(false);
                collision.gameObject.GetComponent<Koopa>().SetIsMoving(false);

                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);

                FindObjectOfType<ScoreCounter>().AddScore(100);

                FindObjectOfType<AudioManager>().Play("Bump");
            }

            else
            {
                if (collision.gameObject.GetComponent<Koopa>().GetIsMoving())
                {
                    if (isBig)
                    {
                        if (canShoot)
                        {
                            canShoot = false;

                            FindObjectOfType<AudioManager>().Play("PowerDown");
                        }
                        else
                        {
                            GetComponent<BoxCollider2D>().size = smolMarioPrefab.GetComponent<BoxCollider2D>().size;

                            isBig = false;

                            FindObjectOfType<AudioManager>().Play("PowerDown");
                        }
                    }
                    else
                    {
                        StartDeath();
                    }
                }
            }
            
        }

        if (collision.gameObject.tag == "PPlant")
        { 
            if (isBig)
            {
                if (canShoot)
                {
                    canShoot = false;

                    FindObjectOfType<AudioManager>().Play("PowerDown");
                }
                else
                {
                    GetComponent<BoxCollider2D>().size = smolMarioPrefab.GetComponent<BoxCollider2D>().size;

                    isBig = false;

                    FindObjectOfType<AudioManager>().Play("PowerDown");
                }
            }
            else
            {
                StartDeath();
            }
        }
    

        if (collision.gameObject.name.Contains("Mushroom"))
        {
            FindObjectOfType<AudioManager>().Play("Powerup");

            if (!isBig)
            {
                Destroy (collision.gameObject);

                isBig = true;

                GetComponent<BoxCollider2D>().size = bigMarioPrefab.GetComponent<BoxCollider2D>().size;
            }
            else
            {
                FindObjectOfType<ScoreCounter>().AddScore(500);

                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.name.Contains("FireFlower"))
        {
            FindObjectOfType<AudioManager>().Play("Powerup");

            if (!isBig || isBig)
            {
                Destroy (collision.gameObject);

                isBig = true;

                GetComponent<BoxCollider2D>().size = bigMarioPrefab.GetComponent<BoxCollider2D>().size;

                canShoot = true;
            }
            if (canShoot)
            {
                FindObjectOfType<ScoreCounter>().AddScore(500);

                Destroy(collision.gameObject);
            }
        }
    }
    public bool GetIsRunning()
    {
        return isRunning;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public bool GetIsBig()
    {
        return isBig;
    }
}

