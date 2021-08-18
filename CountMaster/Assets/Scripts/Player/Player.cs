using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public bool canPlay = false;
    public PopulateCrowd crowd;
    private void Start()
    {
        GameManager._instance.levelStart += GameStart;
        GameManager._instance.levelFinish += Gamefinish;
        if (GameManager._instance.isGameStart)
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        anim.SetFloat("Blend", .5f);
        canPlay = true;
    }

    void Gamefinish(bool isComplete)
    {
        if (isComplete)
        {
            anim.SetFloat("Blend", 1);
            canPlay = false;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (canPlay)
        {
            if (col.transform.CompareTag("hurdle"))
            {
                transform.parent = null;
                anim.SetFloat("Blend", 0);
                crowd.PlayerDeduct();
                canPlay = false;
            }
        }

    }
}
