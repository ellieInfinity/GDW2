using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] GameObject mario;
    [SerializeField] List<GameObject> goombas;
    [SerializeField] List<GameObject> koopas;

    Animator marioAnimator;
    MarioController marioMovement;

    // Start is called before the first frame update
    void Start()
    {
        marioAnimator = mario.GetComponent<Animator>();
        marioMovement = mario.GetComponent<MarioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (marioMovement.GetIsRunning())
        {
            marioAnimator.SetBool("isWalking", true);
        }
        else
        {
            marioAnimator.SetBool("isWalking", false);
        }

        if (marioMovement.GetIsDead())
        {
            marioAnimator.SetBool("isDead", true);
        }
        else
        {
            marioAnimator.SetBool("isDead", false);
        }

        if (marioMovement.GetIsGrounded())
        {
            marioAnimator.SetBool("isJumping", false);
        }
        else
        {
            marioAnimator.SetBool("isJumping", true);
        }

        if (marioMovement.GetIsBig())
        {
            marioAnimator.SetBool("isBig", true);
        }
        else
        {
            marioAnimator.SetBool("isBig", false);
        }

        for (int i = 0; i < goombas.Count; i++)
        {
            if (goombas[i] != null)
            {
                if (goombas[i].GetComponent<Goomba>().GetIsSquashed())
                {
                    goombas[i].GetComponent<Animator>().SetBool("Squashed", true);
                }
                else
                {
                    goombas[i].GetComponent<Animator>().SetBool("Squashed", false);
                }
            }
        }

        for (int i = 0; i < koopas.Count; i++)
        {
            if (koopas[i] != null)
            {
                if (koopas[i].GetComponent<Koopa>().GetIsSquashed())
                {
                    koopas[i].GetComponent<Animator>().SetBool("isSquashed", true);
                }
                else
                {
                    koopas[i].GetComponent<Animator>().SetBool("isSquashed", false);
                }

                if (koopas[i].GetComponent<Koopa>().GetIsKicked())
                {
                    koopas[i].GetComponent<Animator>().SetBool("isKicked", true);
                }
                else
                {
                    koopas[i].GetComponent<Animator>().SetBool("isKicked", false);
                }
            }
        }
    }
}
